# lgd-tool
This repo contains a bunch of unfinished tools to work with `.LGD` files, which are used to create a Windows 2.x boot screen:

![Windows 2.01 boot screen](https://betawiki.net/images/2/28/2.01-Splash.png)

To be more specific, the file consists of several null-terminated strings that contain product and copyright information, followed by a monochrome bitmap, which was seldom different from the stock Microsoft logo.

Please note that even though the Windows 1.x bootscreen looks exactly like Windows 2.x, this tool can't be used for the former, as the logo data that would have been stored inside a `.LGD` file in Windows 2.x is hardcoded inside the `.LGO` file in Windows 1.x. See the Background section for more information.

## Background
During the installation of Windows 2.x, Setup would combine `WIN.CNF`, which contained common Windows initialization code, a `.LGO` file, which contained display-specific code to display the boot screen, and the common `.LGD` file, which contained the boot screen resources, into a single monolithic `WIN.COM` (286 versions) or `WIN86.COM` (386 versions) executable file. 

This technique of combining common and display-specific parts of `WIN.COM` was used also used Windows 3.x, however, the display-independent `.LGD` resource file was replaced by display-specific RLE-encoded bitmaps, which were decoded and rendered by code in the `.LGO` file. Thanks to the modular nature, it is possible to combine `WIN.CNF` from Windows 2.x with a `.LGO` file and corresponding RLE-encoded bitmap from Windows 3.x and use that to boot into Windows 2.x and vice versa.

## Included tools
### `lgd-tool`
This application accepts path to an `.LGD` file as a command line option and creates a text file with product and copyright information and a bitmap file containing the logo extracted from the file.

### `lgd-viewer`
This is a WinForms application that attempts to display `.LGD` files (i.e. simulates the code that would have been provided in an `.LGO` file) in a window.
