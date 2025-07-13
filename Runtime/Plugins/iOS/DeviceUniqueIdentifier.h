#import <Foundation/Foundation.h>

@interface DeviceUniqueIdentifier : NSObject

#ifdef __cplusplus
extern "C" 
{
    #endif
        char * DeviceUniqueId();
    #ifdef __cplusplus
}
#endif

@end