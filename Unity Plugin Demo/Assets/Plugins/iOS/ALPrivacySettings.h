//
//  ALPrivacySettings.h
//  AppLovinSDK
//
//  Created by Basil Shikin on 3/26/18.
//

#import <Foundation/Foundation.h>
#import "ALAnnotations.h"

AL_ASSUME_NONNULL_BEGIN

/**
 * This class contains privacy settings for AppLovin.
 */
@interface ALPrivacySettings : NSObject

/**
 * Set whether or not user has provided consent for information sharing with AppLovin.
 *
 * @param hasUserConsent 'true' if the user has provided consent for information sharing with AppLovin. 'false' by default.
 */
+ (void)setHasUserConsent:(BOOL)hasUserConsent;

/**
 * Check if user has provided consent for information sharing with AppLovin.
 */
+ (BOOL)hasUserConsent;

/**
 * Mark user as one that falls under protection of the USA Children's Online Privacy Protection Act (COPPA).
 *
 * @param userIsCOPPA 'true' if the user falls under protection of COPPA. 'false' by default.
 */
+ (void)setUserIsCOPPA:(BOOL)userIsCOPPA;

/**
 * Check if user falls under protection of the USA Children's Online Privacy Protection Act (COPPA).
 */
+ (BOOL)isUserCOPPA;


- (instancetype)init NS_UNAVAILABLE;

@end

AL_ASSUME_NONNULL_END
