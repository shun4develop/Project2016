//#if ! __has_feature(objc_arc)
//#error This file must be compiled with ARC. Either turn on ARC for the project or use -fobjc-arc flag
//#endif

#import "UnityAppController.h"

extern "C"
{
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
}

char* MakeStringCopy(const char* string)
{
    if (string == NULL) return NULL;
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

@interface ExpandAppController : UnityAppController
@end

@implementation ExpandAppController
- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation
{
    NSString* message = [url absoluteString];
    UnitySendMessage("Receiver", "TriggerOpenURL", MakeStringCopy([message UTF8String]));
    return YES;
}
@end

IMPL_APP_CONTROLLER_SUBCLASS(ExpandAppController)