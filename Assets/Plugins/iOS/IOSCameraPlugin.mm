#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <AssetsLibrary/AssetsLibrary.h>

@interface IOSCameraPlugin : NSObject<UIImagePickerControllerDelegate>
@property(nonatomic, retain)UIImagePickerController *_ipc;
@property(nonatomic, retain)NSMutableArray *_cameraBacks;
@property(nonatomic, copy)NSString *_imagePath;
@property(nonatomic, assign)int _photoStyle;
@property (strong, nonatomic)NSMutableString *mstSelectedImage;
@end

// @interface presentViewController : NSObject<UIAlertController>
// @end

@implementation IOSCameraPlugin
@synthesize _ipc;
@synthesize _cameraBacks;
@synthesize _imagePath;
@synthesize _photoStyle;

/**
 * 初期化
 */
- (id)init {
    self = [super init];
    if (self != nil) {
        // リスト初期化
        _cameraBacks = [NSMutableArray array];
    }
    return self;
}

/**
 * カメラ表示
 * @param imagePath 画像保存パス
 * @param photoStyle フォトスタイル。0：長方形、1：正方形
 */
- (void)showCamera{
    // UIImagePickerController初期化
    _ipc = [[UIImagePickerController alloc] init];
    _ipc.sourceType = UIImagePickerControllerSourceTypeCamera;
    _ipc.delegate = self;
    _ipc.showsCameraControls = YES;
    _ipc.allowsEditing = NO;

    [UnityGetGLViewController() presentViewController:_ipc animated:YES completion:nil];
}
/**
 * カメラ撮影
 */
- (void)takeCamera {
    [_ipc takePicture];
}

/**
 * カメラ非表示
 */
- (void)hideCamera {
    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];
    [self releaseImagePicker];
}

/**
 * アルバム表示
 * @param imagePath 画像保存パス
 * @param photoStyle フォトスタイル。0：長方形、1：正方形
 */
- (void)showAlbum {
    _ipc = [[UIImagePickerController alloc] init];
    _ipc.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    _ipc.allowsEditing = NO;
    _ipc.delegate = self;
    [UnityGetGLViewController() presentViewController:_ipc animated:YES completion:nil];
}

/**
 * フォト保存
 * @param path 保存パス
 */
- (void)savePhoto:(NSString *)path {
    UIImageWriteToSavedPhotosAlbum([UIImage imageWithContentsOfFile:path], self, nil, nil);
}

/**
 * UIImagePickerController：画像選択完了
 * @param picker UIImagePickerController
 * @param info 情報
 */
- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info{
    // ローディングを先に走らせる(画像処理に時間が掛かるため)

    NSLog(@"amano2 %ld", (long)UIImagePickerControllerSourceTypeCamera);    //1
    NSLog(@"amano3 %ld", (long)UIImagePickerControllerSourceTypeSavedPhotosAlbum);  //2
    NSLog(@"amano4 %ld", (long)UIImagePickerControllerSourceTypePhotoLibrary);  //0
    NSLog(@"amano5 %ld", (long)picker.sourceType);


    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];

    // 画像取得
    UIImage *origin = [info objectForKey:UIImagePickerControllerOriginalImage];
    if(origin.size.width > 2048 || origin.size.height > 2048) {
            origin = [self resizedImage:origin size:CGSizeMake(origin.size.width / 2, origin.size.height / 2)];
    }
    //カメラモードの時だけ保存する
    if((long)picker.sourceType == 1){
        UIImageWriteToSavedPhotosAlbum(origin, self, nil, nil);
    }
    UIGraphicsBeginImageContext(origin.size); 
    [origin drawInRect:CGRectMake(0, 0, origin.size.width, origin.size.height)]; 
    origin = UIGraphicsGetImageFromCurrentImageContext(); 
    UIGraphicsEndImageContext();

    // 端末に保存
    NSData *data = UIImagePNGRepresentation(origin);
    NSArray *path = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsDirectory = [path objectAtIndex:0];
    
    // 適当なファイル名をつける.
    NSString *filePath = [documentsDirectory stringByAppendingPathComponent:@"tmp.jpg"];
    [data writeToFile:filePath atomically:YES];

    _imagePath = (NSMutableString *)filePath;

    // Unity側に情報を返す
    //_mstSelectedImage
    UnitySendMessage("MarkerButton", "onCallBack", [self parseStr:_imagePath.UTF8String ]);

    // 破棄
    [self releaseImagePicker];
}

/**
 * 画像リサイズ
 * @param image UIImage
 * @param size リサイズサイズ
 * @return UIImage
 */
- (UIImage *)resizedImage:(UIImage *)image size:(CGSize)size {
    UIGraphicsBeginImageContext(size);
    CGContextRef context = UIGraphicsGetCurrentContext();
    CGContextSetInterpolationQuality(context, kCGInterpolationHigh);
    [image drawInRect:CGRectMake(0, 0, size.width, size.height)];
    image = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    return image;
}
/**
 * UIImagePickerController：画像選択キャンセル
 * @param picker UIImagePickerController
 */
- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker{
    [self hideCamera];
}

/**
 * UIImagePickerController解放
 */
- (void)releaseImagePicker {
    self._ipc = nil;
    [_cameraBacks removeAllObjects];
}

/**
 * 文字列パース
 * @param str 文字列
 * @return 文字列
 */
- (char *)parseStr:(const char *)str {
    if (str == NULL) { return NULL; }
    char *res = (char *)malloc(strlen(str) + 1);
    strcpy(res, str);
    return res;
}
- (void)alertTest {
 
     UIAlertController *alertController = [UIAlertController alertControllerWithTitle:@"モード選択" message:@"どれにしますか？" preferredStyle:UIAlertControllerStyleActionSheet];
    
//     // 上から順にボタンが配置
       [alertController addAction:[UIAlertAction actionWithTitle:@"カメラ" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
          [self selectedActionWith:1];
       }]];
    [alertController addAction:[UIAlertAction actionWithTitle:@"カメラロール" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        [self selectedActionWith:2];
    }]];
    [alertController addAction:[UIAlertAction actionWithTitle:@"クリア" style:UIAlertActionStyleCancel handler:^(UIAlertAction *action) {
        [self selectedActionWith:0];
    }]];
    
//     // iPad用　（これが無いとエラー）
//     // alertController.popoverPresentationController.sourceView = self.view;
//     // alertController.popoverPresentationController.sourceRect = CGRectMake(_actionSheetBtn.frame.origin.x, _actionSheetBtn.frame.origin.y, 20.0, 20.0);
//     //alertController.popoverPresentationController.sourceView = _actionSheetBtn; //でも良い
    
    UIViewController *baseView = [UIApplication sharedApplication].keyWindow.rootViewController;
    while (baseView.presentedViewController != nil && !baseView.presentedViewController.isBeingDismissed) {
        baseView = baseView.presentedViewController;
    }
     [baseView presentViewController:alertController animated:YES completion:nil];
   
}
 
-(void)selectedActionWith:(int)index{
    // 選択時の処理
    switch(index){
        case 1:
            [self showCamera];
        break;
        case 2:
            [self showAlbum];
        break;
        case 0:
        break;
    }
}
@end

/**
 * ネイティブメソッド
 */
extern "C" {
    static IOSCameraPlugin *plugin =[[IOSCameraPlugin alloc] init];
    UIView *UnityGetGLView();
    UIViewController *UnityGetGLViewController();
    void UnitySendMessage(const char *, const char *, const char *);

    static NSString *getStr(char *str){
        if (str) {
            return [NSString stringWithCString: str encoding:NSUTF8StringEncoding];
        } else {
            return [NSString stringWithUTF8String: ""];
        }
    }

    void showCamera(){
        [plugin showCamera];
    }

    void showAlbum(){
        [plugin showAlbum];
    }

    void savePhoto(char *path){
        [plugin savePhoto:getStr(path)];
    }

    void alertTest(){
        [plugin alertTest];
    };


}
















