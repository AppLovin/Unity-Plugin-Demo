//
//  ALAppLovinLogger.m
//  Unity Plugin
//
//  Created by Christopher Cong on 9/10/18.
//  Copyright © 2018 AppLovin. All rights reserved.
//

#import "ALAppLovinLogger.h"

@interface ALAppLovinLogger()
@property (nonatomic, weak) ALSdk *sdk;
@end

@implementation ALAppLovinLogger
static NSString *const SDK_TAG = @"AppLovinSDK";
static NSString *const SOURCE = @"AppLovinUnity";

- (instancetype)initWithSdk:(ALSdk *)sdk
{
    self = [super init];
    if ( self )
    {
        self.sdk = sdk;
    }
    return self;
}

- (void)d:(NSString *)format, ...
{
    va_list valist;
    va_start(valist, format);
    NSString *message = [[NSString alloc] initWithFormat: format arguments: valist];
    va_end(valist);
    
    [self logMessage: message atLevel: @"DEBUG"];
}

- (void)i:(NSString *)format, ...
{
    va_list valist;
    va_start(valist, format);
    NSString *message = [[NSString alloc] initWithFormat: format arguments: valist];
    va_end(valist);
    
    [self logMessage: message atLevel: @"INFO"];
}

- (void)w:(NSString *)format, ...
{
    va_list valist;
    va_start(valist, format);
    NSString *message = [[NSString alloc] initWithFormat: format arguments: valist];
    va_end(valist);
    
    [self logMessage: message atLevel: @"WARN"];
}


- (void)e:(NSString *)message becauseOf:(nullable NSException *)exception
{
    NSString *messageToReport;
    if ( exception )
    {
        messageToReport = [NSString stringWithFormat: @"%@ : %@", message, exception];
        
        [self logMessage: messageToReport atLevel: @"ERROR"];
    }
    else
    {
        messageToReport = [NSString stringWithFormat: @"%@", message];
        
        [self logMessage: messageToReport atLevel: @"ERROR"];
    }
}

- (void)e:(NSString *)format, ...
{
    va_list valist;
    va_start(valist, format);
    NSString *messageToReport = [[NSString alloc] initWithFormat: format arguments: valist];
    va_end(valist);
    
    [self e: messageToReport becauseOf: nil];
}

- (void)userError:(NSString *)format, ...
{
    va_list valist;
    va_start(valist, format);
    NSString *message = [[NSString alloc] initWithFormat: format arguments: valist];
    va_end(valist);
    
    NSString *logMessage = [NSString stringWithFormat: @"USER [%@] %@", SOURCE, message];
    
    NSLog(@"[%@] %@", SDK_TAG, logMessage);
}

- (void)logMessage:(NSString *)message atLevel:(NSString *)level
{
    NSString *logMessage = [NSString stringWithFormat: @"%@ [%@] %@", level, SOURCE, message];
    if ( [self isVerbose] )
    {
        NSLog(@"[%@] %@", SDK_TAG, logMessage);
    }
}

#pragma mark - Utility Methods

- (BOOL)isVerbose
{
    return self.sdk.settings.isVerboseLogging;
}

@end
