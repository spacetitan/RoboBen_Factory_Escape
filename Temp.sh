#!/bin/sh
echo -ne '\033c\033]0;Temp\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/Temp.x86_64" "$@"
