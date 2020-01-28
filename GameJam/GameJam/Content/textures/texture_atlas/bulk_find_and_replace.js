#!/usr/bin/env node

const fs = require('fs');

let contents = fs.readFileSync('CompleteTextureAtlas.json', 'utf8');

let csvContents = fs.readFileSync('map.csv', 'utf8');
console.log(typeof(csvContents));
let csvLines = csvContents.split('\n');
for(let i = 0; i < csvLines.length; i++) {
	if(csvLines[i].trim().length == 0) {
		continue;
	}
	let lineParts = csvLines[i].split(',');
	let oldFind = lineParts[0];
	let newReplace = lineParts[1];
	console.log(`Replacing ${oldFind} with ${newReplace}`);
	contents = contents.split(oldFind).join(newReplace);
}

// Rewrite
fs.writeFileSync('CompleteTextureAtlas.json', contents);
console.log('Done!');
