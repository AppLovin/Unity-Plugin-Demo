//
//  ALAdDelegateWrapper.h
//  Unity-iPhone
//
//  Created by Matt Szaro on 1/16/14.
//
//

#import <Foundation/Foundation.h>
#import "ALManagedLoadDelegate.h"
#import "ALAppLovinLogger.h"

#if __has_include(<AppLovinSDK/AppLovinSDK.h>)
    #import <AppLovinSDK/AppLovinSDK.h>
#else
    #import "ALAdView.h"
    #import "ALAdLoadDelegate.h"
    #import "ALAdDisplayDelegate.h"
    #import "ALAdVideoPlaybackDelegate.h"
    #import "ALAdRewardDelegate.h"
    #import "ALAdViewEventDelegate.h"
#endif

@interface ALAdDelegateWrapper : NSObject <ALAdLoadDelegate, ALAdDisplayDelegate, ALAdRewardDelegate, ALAdVideoPlaybackDelegate, ALUnityTypedLoadFailureDelegate, ALAdViewEventDelegate>

@property (assign, atomic) BOOL isInterstitialShowing;

@property (strong, nonatomic) NSString* gameObjectToNotify;

- (instancetype)initWithLogger:(ALAppLovinLogger *)logger;

- (instancetype)init NS_UNAVAILABLE;

@end
