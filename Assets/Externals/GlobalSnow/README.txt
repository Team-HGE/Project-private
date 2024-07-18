**************************************
*            GLOBAL SNOW             *
*        Created by Kronnect         * 
*            README FILE             *
**************************************


How to use this asset
---------------------
Firstly, you should run the Demo Scene provided to get an idea of the overall functionality.
Later, please read the documentation and experiment with the system.

To quick start using the asset:
- Add the Global Snow Render Feature to your URP Universal Renderer asset.
- Add Global Snow script to your camera. Snow should appear in the Game View. Customize it using the custom inspector.


Requirements
------------
- Unity 2021.3 or later
- URP
- Deferred rendering path


Documentation/API reference
---------------------------
The PDF is located in the Documentation folder. It contains instructions on how to use this asset as well as a useful Frequent Asked Question section.


Support
-------
Please read the documentation PDF and browse/play with the demo scene and sample source code included before contacting us for support :-)

Have any question or issue?
* Support-Web: https://kronnect.com/support
* Support-Discord: https://discord.gg/EH2GMaM
* Email: contact@kronnect.com
* Twitter: @Kronnect


Future updates
--------------

All our assets follow an incremental development process by which a few beta releases are published on our support forum (kronnect.com).
We encourage you to signup and engage our forum. The forum is the primary support and feature discussions medium.

Of course, all updates of Global Snow will be eventually available on the Asset Store.


Version history
---------------

Version 10.1.1
- [Fix] Global illumination option fixes

Version 10.1
- API: added MaskPaint(bounds) variant with opacity and falloff options
- [Fix] Fixes to coverage mask editor

Version 10.0
- Added support for Render Graph (Unity 2023.3)
- Added "CamerasLayerMask" property to the Global Snow Render Feature. Useful to ignore certain cameras from the snow effect.
- Added "Minimum Ambient Intensity" property used when "Preserve Global Illumination" option is disabled
- API: added OnBeforeUpdateCoverage / OnPostUpdateCoverage events

Version 8.5.1
- Change: Coverage Mask no uses a single channel (R8) texture format. Use red channel to specify coverage value instead of alpha.
- API: MaskPaint() method improved with a MaskPaintMethod parameter that supports GPU operation for faster execution

Version 8.5
- API: added MaskPaint(): paints or clears snow on mask
- API: added MaskClear(): fills the mask texture with a constant value
- API: added MaskFillArea(): fills an area with snow equal to the object shape

Version 8.4
- Exposed "Exclusion Depth Bias" option

Version 8.3
- Optimization: exclusion buffer is now automatically ignored if no excluded objects are visible

Version 8.2
- Added "GlobalSnow/Moving Objects Snow/Opaque" shader
- Minor fixes and internal improvements

Version 8.1.2
- Mask texture sampling is now done separately from the zenithal coverage, allowing larger mask coverage on terrains
- Inspector will warn when using a coverage mask with wrong format and allows to automatically fix it

Feb/2023 - First URP release

