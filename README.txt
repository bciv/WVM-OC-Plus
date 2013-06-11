Project: 	Wmvocplus

Description: 	Wmvocplus is a command-line utility for overlaying open captioned text 
into Windows Media Video (WMV) formatted video files.

Author:	 Will Collins IV  will@etherfeat.com

Created on: 2010-12-16

Requisites:
* A computer running Windows XP, VistA, 7, or 8 is required
* The computer must have Microsoft Expression Encoder 4 installed

Usage:

Wmvocplus <wmvfilename> <JPG filler to append> <seconds to display filler>

Options:

Wmvfilename – the name of the video that needs open captioning.  The video file MUST be 
              accompanied by an SMI file containing the text that needs to be burned into 
              the video as ‘open captioned’ text

JPG filler to append – an optional image file that can statically play when the video is 
              over ~useful if the length of the video needs to be extended for scheduling 
              purposes

Seconds to display filler – The number of seconds to play a JPG filler


