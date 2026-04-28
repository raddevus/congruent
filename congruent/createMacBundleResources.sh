APP_NAME="congruent"
PUBLISH_DIR="bin/Release/net10.0/osx-arm64/publish"
APP_DIR="congruent.app"

rm -rf "$APP_DIR"
mkdir -p "$APP_DIR/Contents/MacOS"
mkdir -p "$APP_DIR/Contents/Resources"

cp Info.plist "$APP_DIR/Contents/Info.plist"
cp Assets/congruent.icns "$APP_DIR/Contents/Resources/congruent.icns"
cp -a "$PUBLISH_DIR"/. "$APP_DIR/Contents/MacOS/"
mv congruent.app/Contents/MacOS/congruent congruent.app/Contents/MacOS/congruent
