// @ts-check
'use strict';

/**
 * This is a data mining script to get information from unpacked SGA archives for the base
 * dawn of war. Extracting info from SGA at runtime is difficult and would be too slow anyway so we'll just
 * bundle the base game info we need with this application when publishing.
 *
 * To use this:
 * 1) Unpack an SGA archive into some folder
 * 2) Copy and paste the Locale folder into the same folder you extracted all the data too
 * 3) Run Script with absolute path to extracted folder
 */

const fs = require('fs');
const path = require('path');
const lua = require('luaparse');

if (process.argv.length < 3) {
  console.log('Missing argument: <dir>');
  process.exit(1);
}

const dir = process.argv[2];

const locale = {};

const localeDir = path.join(dir, 'Locale', 'English');
if (fs.existsSync(localeDir)) {
  const files = getFilesMatching(localeDir, /\.ucs$/);
  for (const filePath of files) {
    const ucsData = fs.readFileSync(filePath, { encoding: 'ucs2' });
    ucsData.split(/\n+/g)
      .map(line => /^(\d+)\s+(.+)$/g.exec(line.trim()))
      .filter(match => !!match)
      .forEach(match => locale['$' + match[1]] = match[2]);
  }
} else {
  console.error('Missing Locale dir');
  process.exit(1);
}

const wcs = [];

const winConditionDir = path.join(dir, 'scar', 'winconditions');
if (fs.existsSync(winConditionDir)) {
  const files = getFilesMatching(winConditionDir, /_local\.lua$/);
  for (const filePath of files) {
    const winCondition = readWinCondition(filePath);
    if (winCondition) {
      replaceLocales(winCondition);
      wcs.push(winCondition);
    }
  }
} else {
  console.log('No win conditions directory');
}

let maps = [];

const scenariosDir = path.join(dir, 'scenarios', 'mp');
if (fs.existsSync(scenariosDir)) {
  const files = getFilesMatching(scenariosDir, /\.sgb$/);
  for (const filePath of files) {
    const map = readMap(filePath);
    if (map) {
      replaceLocales(map);
      maps.push(map);
    }
  }
} else {
  console.log('No scenarios directory');
}

if (maps.length || wcs.length) {
  // maps = maps.sort((a, b) => a.players === b.players ? a.name.localeCompare(b.name) : a.players - b.players);
  fs.writeFileSync('data.json', JSON.stringify({ maps, wcs }, null, 2), { encoding: 'utf8' });
} else {
  console.log('No data found');
}

function getFilesMatching(filePath, type) {
  return fs.readdirSync(filePath, { withFileTypes: true })
      .filter(file => file.isFile())
      .filter(file => file.name.match(type))
      .map(file => path.join(filePath, file.name));
}

function replaceLocales(obj) {
  const keys = Object.keys(obj)
    .filter(key => typeof obj[key] === 'string')
    .filter(key => /^\$\d+$/.test(obj[key]));

  if (keys.length > 0) {
    keys.forEach(key => {
      if (locale[obj[key]]) {
        obj[key] = locale[obj[key]];
      }
    });
  }
}

/**
 * @param {string} filePath
 */
function readMap(filePath) {
  const stripExt = filePath.replace(/\.sgb$/, '');

  let imageLink;
  if (fs.existsSync(`${stripExt}_icon_custom.tga`)) {
    imageLink = `${stripExt}_icon_custom.tga`;
  } else if (fs.existsSync(`${stripExt}_icon.tga`)) {
    imageLink = `${stripExt}_icon.tga`;
  } else {
    console.log(`Probably not valid: ${filePath}`)
    return;
  }

  const mapDetails = getMapDetails(filePath);
  mapDetails.pic = imageLink;

  // All base game maps are prefixed with the # of players the map supports.
  // If the map name # is not the same as the actual value set then there is something fishy here.
  const [_, name] = /([^\\/]+)\.sgb$/g.exec(filePath);
  const expectedPlayers = parseInt(name[0]);
  if (expectedPlayers !== mapDetails.players) {
    console.log(`Ignoring bad file: ${filePath}`);
    return;
  }

  return mapDetails;
}

/**
 * Chunky files are LITTLE-ENDIAN
 *
 * Chunk Header Format:
 * 4 byte chunk type "DATA"
 * 4 byte chunk ID "WHMD"
 * 4 byte chunk version
 * 4 byte chunk size
 * 4 byte name size
 * X byte name
 *
 * Map Chunk Format:
 * 4 byte player size
 * 4 byte map size
 * 4 byte mod/folder/something size
 * X byte mod/folder/something
 * 4 16-bit map name size
 * X byte map
 * 4 16-bit map description size
 * X byte description
 */
function getMapDetails(filePath) {
  let sgbBuffer = fs.readFileSync(filePath);
  if (sgbBuffer.toString('utf8', 0, 12) !== 'Relic Chunky') {
    throw 'Buffer is not a relic chunky';
  }

  // First we need to find the 'DATAWMHD' chunky type/id as this is where the metadata about the map is stored
  // I don't know if it's possible to find this without manually searching byte-by-byte
  const headerOffset = sgbBuffer.indexOf('DATAWMHD', 11, 'utf8');
  if (headerOffset < 0) {
    throw 'Buffer is not a valid SGB';
  }

  // The namesize is the number of bytes given to the name of this chunky header
  // Using this we calculate the offset where the meta data begins
  const namesize = sgbBuffer.readInt32LE(headerOffset + 16);
  const chunkOffset = headerOffset + 20 + namesize;

  const players = sgbBuffer.readInt32LE(chunkOffset);
  const mapSize = sgbBuffer.readInt32LE(chunkOffset + 4);
  const modNameSize = sgbBuffer.readInt32LE(chunkOffset + 8);

  const textOffset = chunkOffset + 12 + modNameSize;

  // Multiply by 2 because it's refering to UTF-16 characters
  const mapNameSize = sgbBuffer.readInt32LE(textOffset) * 2;
  const mapNameOffset = textOffset + 4;
  const mapName = sgbBuffer.toString('utf16le', mapNameOffset, mapNameOffset + mapNameSize);

  const mapDescSize = sgbBuffer.readInt32LE(mapNameOffset + mapNameSize) * 2;
  const mapDescOffset = mapNameOffset + mapNameSize + 4;
  const mapDesc = sgbBuffer.toString('utf16le', mapDescOffset, mapDescOffset + mapDescSize);

  return { name: mapName, description: mapDesc, players: players, size: mapSize, pic: '' };
}

/**
 * @param {string} filePath
 */
function readWinCondition(filePath) {
  const luaData = lua.parse(fs.readFileSync(filePath, 'utf8'));
  // @ts-ignore
  const body = luaData.body.find(body => body.type === 'AssignmentStatement'
    && body.variables.every(v => v.type === 'Identifier' && v.name === 'Localization'));
  if (!body) return null;

  // @ts-ignore
  const init = body.init.find(init => init.type === 'TableConstructorExpression');
  if (!init) return null;

  const values = {};
  init.fields.forEach(field => values[field.key.name] = field.value.value);

  return {
    title: values['title'],
    description: values['description'],
    victoryCondition: values['victory_condition'],
    alwaysOn: values['always_on'],
    exclusive: values['exclusive'],
  };
}
