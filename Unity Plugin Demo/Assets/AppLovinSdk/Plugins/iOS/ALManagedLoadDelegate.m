//
//  ALTypeRememberingLoadDelegate.m
//  AppLovin Unity Extension
//
//  Created by Matt Szaro, Thomas So on 5/20/14.
//  Copyright (c) 2014 AppLovin. All rights reserved.
//

#import "ALManagedLoadDelegate.h"
#import "ALInterstitialCache.h"

#if __has_include(<AppLovinSDK/AppLovinSDK.h>)
    #import <AppLovinSDK/AppLovinSDK.h>
#else
    #import "ALInterstitialAd.h"
#endif

@interface ALManagedLoadDelegate ()
@property (strong, nonatomic) id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate> wrapper;
@end

@implementation ALManagedLoadDelegate

static NSMutableSet *managedLoadDelegates;
static NSObject *managedLoadDelegatesLock;

#pragma mark - Initialization

- (instancetype)initWithZoneIdentifier:(NSString *)zoneIdentifier
                                  size:(ALAdSize *)size
                                  type:(ALAdType *)type
                               wrapper:(id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>)wrapper
{
    self = [super init];
    if(self)
    {
        _zoneIdentifier = zoneIdentifier;
        _size = size;
        _type = type;
        
        self.wrapper = wrapper;
    }
    return self;
}

#pragma mark - Public Methods

+ (instancetype)sharedDelegateForZoneIdentifier:(NSString *)zoneIdentifier
                                           size:(ALAdSize *)size
                                           type:(ALAdType *)type
                                        wrapper:(id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>)wrapper
{
    if ( !managedLoadDelegates ) managedLoadDelegates = [NSMutableSet set];
    if ( !managedLoadDelegatesLock ) managedLoadDelegatesLock = [[NSObject alloc] init];
    
    ALManagedLoadDelegate *delegate;
    
    @synchronized ( managedLoadDelegatesLock )
    {
        delegate = [self retrieveDelegateForZoneIdentifier: zoneIdentifier size: size type: type wrapper: wrapper];
        if ( !delegate )
        {
            delegate = [self createDelegateForZoneIdentifier: zoneIdentifier size: size type: type wrapper: wrapper];
        }
    }
    
    return delegate;
}

+ (ALManagedLoadDelegate *)retrieveDelegateForZoneIdentifier: (NSString *)zoneIdentifier
                                                        size: (ALAdSize *)size
                                                        type: (ALAdType *)type
                                                     wrapper: (id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>)wrapper
{
    for ( ALManagedLoadDelegate *delegate in managedLoadDelegates )
    {
        if ( [zoneIdentifier isEqualToString: delegate.zoneIdentifier] && [delegate.wrapper isEqual: wrapper] )
        {
            return delegate;
        }
    }
    
    return nil;
}

+ (ALManagedLoadDelegate *)createDelegateForZoneIdentifier: (NSString *)zoneIdentifier
                                                      size: (ALAdSize *)size
                                                      type: (ALAdType *)type
                                                   wrapper: (id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>)wrapper
{
    ALManagedLoadDelegate *delegate = [[ALManagedLoadDelegate alloc] initWithZoneIdentifier: zoneIdentifier size: size type: type wrapper: wrapper];
    [managedLoadDelegates addObject: delegate];
    
    return delegate;
}

#pragma mark - Ad Load Delegate Methods

- (void)adService:(ALAdService *)adService didLoadAd:(ALAd *)ad
{
    // If this delegate is for a zone, save this ad in the zone ad cache
    if ( self.cacheInterstitialOnLoad )
    {
        [[ALInterstitialCache shared] setAd: ad forZoneIdentifier: self.zoneIdentifier];
    }
    
    // If showing of an ad was requested upon load
    if ( self.showInterstitialOnLoad )
    {
        // Show the ad
        [[ALInterstitialAd shared] showAd: ad];
    }
    // If showing of a banner ad was requested upon load
    else if ( self.showBannerOnLoad )
    {
        if ( ![self.adView.adSize isEqual: ad.size] )
        {
            CGFloat w, h;
            
            if ( [[ALAdSize sizeMRec] isEqual: ad.size] )
            {
                w = 300.0f;
                h = 250.0f;
            }
            else // BANNER or LEADER
            {
                h = [[ALAdSize sizeBanner] isEqual: ad.size] ? 50.0f : 90.0f;
                w = [self getAvailableScreenWidth];
            }
            
            CGRect newRect = self.adView.frame;
            newRect.size.width = w;
            newRect.size.height = h;
            self.adView.frame = newRect;
        }
        
        [self.adView render: ad];
    }
    
    // Forward this through to the wrapper.
    [self.wrapper adService: adService didLoadAd: ad];
}

- (void)adService:(ALAdService *)adService didFailToLoadAdWithError:(int)code
{
    // Pass this typed load fail info to the wrapper.
    [self.wrapper adService: adService didFailToLoadAdOfSize: self.size type: self.type withError: (NSInteger)code];
}

#pragma mark - NSObject Overrides

- (NSString *)description
{
    return [NSString stringWithFormat: @"%@ - %@/%@/%@", @"ALManagedLoadDelegate", self.size, self.type, self.wrapper];
}

- (CGFloat)getAvailableScreenWidth
{
    CGRect screenBounds = [UIScreen mainScreen].bounds;
    
    UIInterfaceOrientation orientation = [[UIApplication sharedApplication] statusBarOrientation];
    
    CGFloat width = screenBounds.size.width;
    
    // Don't trust the system
    if ((UIInterfaceOrientationIsLandscape(orientation) && screenBounds.size.height > screenBounds.size.width) || (UIInterfaceOrientationIsPortrait(orientation) && screenBounds.size.width > screenBounds.size.height))
    {
        width = screenBounds.size.height;
    }
    
    return width;
}

@end
