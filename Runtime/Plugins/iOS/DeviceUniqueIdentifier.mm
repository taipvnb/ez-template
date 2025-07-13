#import "DeviceUniqueIdentifier.h"
#import "Keychain.h"

@implementation DeviceUniqueIdentifier

+ (NSString *) GenUUID
{
    CFUUIDRef uuid_ref = CFUUIDCreate(NULL);
    
    CFStringRef uuid_string_ref= CFUUIDCreateString(NULL, uuid_ref);
    
    CFRelease(uuid_ref);
    
    NSString *uuid = [NSString stringWithString:(__bridge NSString*)uuid_string_ref];
    
    CFRelease(uuid_string_ref);
    
    return uuid;
}

+ (NSString *)getuuid
{    
    NSString * serviceName =@"com.unimob.uuid";
    
    NSString * accountName =@"unimob";
    
    // get value
    NSString * value = [Keychain passwordForService:serviceName account:accountName];
    
    if (value==nil) {
        
        // gen uuid
        NSString * uuid =[self GenUUID];

        // save uuid
        [Keychain setPassword:uuid forService:serviceName account:accountName];
        return uuid;
    } else {
        return value;
    }
}

char * DeviceUniqueId()
{
    // get uuid
    const char *uuid = [[DeviceUniqueIdentifier getuuid] UTF8String];
    // alloc
    char *result = (char*)malloc(strlen(uuid)+1);
    // copy
    strcpy(result, uuid);
    // return
    return result;
}

@end