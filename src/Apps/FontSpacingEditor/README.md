# Yakuza Kiwami 2 VWF Editor

App to edit the character spacing table in Yakuza Kiwami 2

## Usage

1. Load `yakuza.dds` image.
2. Load `yakuza_spacing.txt` (exported with TF3).
3. Click on the character you want to edit.
4. Edit the six values using the numeric controls.
5. Repeat step `3` with other character.
6. Save the modified `yakuza_spacing.txt`.

## Automatic calculation

The app can find the "non-black" pixels in the character and calculate the needed values. To do that, just:

4. Choose a margin (in pixels). Usually, just 1 or 2 pixels.
5. Click on the `Calculate` button.
