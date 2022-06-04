#!/bin/sh
#By Nate Flink

#Invoke on the terminal like this
#curl -s https://gist.github.com/nateflink/9056302/raw/findreplaceosx.sh | bash -s "find-a-url.com" "replace-a-url.com"

if [ -z "$1" ]; then
  echo "Usage: ./$0 [find string] [replace string]"
  exit 1
fi

FIND=$1
REPLACE=$2

#needed for byte sequence error in ascii to utf conversion on OSX
export LC_CTYPE=C;
export LANG=C;

#sed -i "" is needed by the osx version of sed (instead of sed -i)
find Unity.Physics.Package -type f -exec sed -i "" "s|${FIND}|${REPLACE}|g" {} +
exit 0