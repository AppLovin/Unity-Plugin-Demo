***PLEASE READ***

3/18/2 - We have migrated our AppLovin Unity Plugin to a new directory structure and for iOS to use a first-class framework instead of raw binary and headers. 

If you are updating from a previous Unity Plugin, all you need to do is delete the following:
1. Assets/Plugins/AppLovin
2. Assets/Plugins/Android/applovin-sdk 
3. Assets/Plugins/Android/applovin-unity-plugin
4. Assets/Plugins/iOS/libAppLovinSdk.a
5. Assets/Plugins/iOS/AppLovinUnity.mm
6. Assets/Plugins/iOS/<ALL APPLOVIN FILES WHICH ARE PREFIXED WITH "AL*" or "MA*">

If there are any questions, please feel free to shoot me an email at thomas.so@applovin.com
