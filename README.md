[![Test](https://github.com/cello-ben/Parallelograph/actions/workflows/test.yaml/badge.svg)](https://github.com/cello-ben/Parallelograph/actions/workflows/test.yaml)

# Parallelograph
If you take theory classes at a Western conservatory, you'll probably be taught what is known as **voice leading**. Simply put, it's the practice of using harmony and counterpoint guidelines given to you by the curriculum to fill in voices in a musical structure ("realizing"). A prohibition that one generally encounters in so-called Common Practice voice leading is that of parallel fifths and octaves. That is, there must not be two perfect fifths in a row, nor two perfect octaves (or parallel unisons, for that matter) between two given voices. This is a simple utility to parse a 4-voice, 1-part, homorhythmic MusicXML score and tell the user if it contains any of these forbidden constructs.

This program implements parallel checking with exceptions for voices that remain static or for voice changes.