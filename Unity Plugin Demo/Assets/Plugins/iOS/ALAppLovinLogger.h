//
//  ALAppLovinLogger.h
//  Unity Plugin
//
//  Created by Christopher Cong on 9/10/18.
//  Copyright © 2018 AppLovin. All rights reserved.
//

#import <Foundation/Foundation.h>

#if __has_include(<AppLovinSDK/AppLovinSDK.h>)
    #import <AppLovinSDK/AppLovinSDK.h>
#else
    #import "ALSdk.h"
#endif

NS_ASSUME_NONNULL_BEGIN

@interface ALAppLovinLogger : NSObject

- (void)d:(NSString *)format, ...;
- (void)i:(NSString *)format, ...;
- (void)w:(NSString *)format, ...;
- (void)e:(NSString *)format, ...;
- (void)e:(NSString *)message becauseOf:(nullable NSException *)ex;
- (void)userError:(NSString *)format, ...;

- (instancetype)initWithSdk:(ALSdk *)sdk;

@end

NS_ASSUME_NONNULL_END
