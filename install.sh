#!/bin/bash

echo "installing"

mkdir -p ~/.local/bin
cp ./clidle/bin/Debug/net10.0/* ~/.local/bin/
cp ./clidle/valid_guesses.txt ~/
cp ./clidle/words.txt ~/

echo "done"
