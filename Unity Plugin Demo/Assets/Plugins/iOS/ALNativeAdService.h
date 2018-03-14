//
//  ALNativeAdService.h
//  sdk
//
//  Created by Thomas So on 5/21/15.
//
//

#import <Foundation/Foundation.h>
#import "ALAnnotations.h"
#import "ALNativeAdLoadDelegate.h"
#import "ALNativeAdPrecacheDelegate.h"

AL_ASSUME_NONNULL_BEGIN

@class ALSdk;
@class ALNativeAd;

@interface ALNativeAdService : NSObject

/**
 *  Load a batch of native ads, which are guaranteed not to repeat, asynchronously.
 *
 *  @param  numberOfAdsToLoad  The number of native ads to load.
 *  @param  delegate           The native ad load delegate to notify upon completion.
 */
- (void)loadNativeAdGroupOfCount:(NSUInteger)numberOfAdsToLoad andNotify:(alnullable id <ALNativeAdLoadDelegate>)delegate;

/**
 *  Load a batch of native ads, which are guaranteed not to repeat, asynchronously.
 *
 *  @param  numberOfAdsToLoad  The number of native ads to load.
 *  @param  zoneIdentifier     The identifier of the zone to load the native ads for.
 *  @param  delegate           The native ad load delegate to notify upon completion.
 */
- (void)loadNativeAdGroupOfCount:(NSUInteger)numberOfAdsToLoad forZoneIdentifier:(alnullable NSString *)zoneIdentifier andNotify:(alnullable id <ALNativeAdLoadDelegate>)delegate;

/**
 *  Pre-cache image and video resources of a native ad.
 *
 *  @param  ad        The native ad whose resources should be cached.
 *  @param  delegate  The delegate to be notified upon completion.
 */
- (void)precacheResourcesForNativeAd:(ALNativeAd *)ad andNotify:(alnullable id <ALNativeAdPrecacheDelegate>)delegate;

/**
 * Pre-load an ad for a given zone in the background, if one is not already available.
 *
 * @param  zoneIdentifier  Identifier representing the zone for the ad to cache.
 */
- (void)preloadAdForZoneIdentifier:(NSString *)zoneIdentifier;

/**
 * Check whether an ad for a given zone is pre-loaded and ready to be displayed.
 *
 * @param zoneIdentifier  Zone for the ad to check for.
 *
 * @return YES if an ad for this zone is pre-loaded and ready to display without further network activity. NO if requesting an ad for this zone would require fetching over the network.
 */
- (BOOL)hasPreloadedAdForZoneIdentifier:(NSString *)zoneIdentifier;


- (instancetype)init __attribute__((unavailable("Don't instantiate ALNativeAdService, access one via [sdk nativeAdService] instead.")));

@end

AL_ASSUME_NONNULL_END
