#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <AssetsLibrary/AssetsLibrary.h>

@interface CharikitaPlugin : NSObject<UIImagePickerControllerDelegate>
@property(nonatomic, retain)UIImagePickerController *_ipc;
@property(nonatomic, retain)NSMutableArray *_cameraBacks;
@property(nonatomic, copy)NSString *_imagePath;
@property(nonatomic, assign)int _photoStyle;
@property (strong, nonatomic)NSMutableString *mstSelectedImage;
@end

// @interface presentViewController : NSObject<UIAlertController>
// @end

@implementation CharikitaPlugin
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
- (void)showCamera:(NSString *)imagePath photoStyle:(int)photoStyle{
    _imagePath = imagePath;
    _photoStyle = photoStyle;
    // UIImagePickerController初期化
    _ipc = [[UIImagePickerController alloc] init];
    _ipc.sourceType = UIImagePickerControllerSourceTypeCamera;
    _ipc.delegate = self;
    _ipc.showsCameraControls = YES;
    _ipc.allowsEditing = NO;



    // カメラビュー
    CGRect s = [[UIScreen mainScreen] bounds];
    float sw = s.size.width;
    float sh = s.size.height;
    float cw = 720 / 2;
    float ch = 1280 / 2;
    float r  = (sh * cw) / (sw * ch);
    float a  = ((r > 1) ? (sh / ch) : (sw / cw)) / 2;
    UIView *parent = [[UIView alloc] initWithFrame:CGRectMake(0, 0, sw, sh)];


    [UnityGetGLViewController() presentViewController:_ipc animated:YES completion:nil];
}

/**
 * UIImageView作成
 * @param parent 親ビュー
 * @param tag タグ
 * @param imageNamed イメージ名
 * @param frame 描画フレーム
 * @return UIImageView
 */
- (UIImageView *)createImageView:(UIView *)parent tag:(int)tag imageNamed:(NSString *)imageNamed frame:(CGRect)frame {
    UIImage *image = [UIImage imageNamed:imageNamed];
    UIImageView *view = [[UIImageView alloc] initWithImage:image];
    [view setTag:tag];
    [view setFrame:frame];
    [parent addSubview:view];
    return view;
}

/**
 * UIButton作成
 * @param parent 親ビュー
 * @param tag タグ
 * @param imageNamed イメージ名
 * @param frame 描画フレーム
 * @param action ボタンクリック時のコールバック
 * @return UIButton
 */
- (UIButton *)createButton:(UIView *)parent tag:(int)tag imageNamed:(NSString *)imageNamed frame:(CGRect)frame action:(SEL)action {
    UIImage *image = [UIImage imageNamed:imageNamed];
    UIButton *button = [UIButton buttonWithType:UIButtonTypeCustom];
    [button setTag:tag];
    [button setImage:image forState:UIControlStateNormal];
    [button setImage:image forState:UIControlStateHighlighted];
    [button setFrame:frame];
    [button.imageView setContentMode:UIViewContentModeScaleAspectFit];
    [button setContentMode:UIViewContentModeScaleAspectFit];
    [button addTarget:self action:action forControlEvents:UIControlEventTouchUpInside];
    [parent addSubview:button];
    return button;
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
 * フォトスタイルを長方形に更新
 */
- (void)updateRectangle {
    [self updatePhotoStyle:true isSquare:false];
}

/**
 * フォトスタイルを正方形に更新
 */
- (void)updateSquare {
    [self updatePhotoStyle:false isSquare:true];
}

/**
 * フォトスタイル更新
 * @param isRectangle 長方形の場合 true
 * @param isSquare 正方形の場合 true
 */
- (void)updatePhotoStyle:(BOOL)isRectangle isSquare:(BOOL)isSquare {
    _photoStyle = (isRectangle) ? 0 : 1;
    ((UIView *)[_cameraBacks objectAtIndex:0]).hidden = isSquare;
    ((UIView *)[_cameraBacks objectAtIndex:1]).hidden = isRectangle;
}

/**
 * アルバム表示
 * @param imagePath 画像保存パス
 * @param photoStyle フォトスタイル。0：長方形、1：正方形
 */
- (void)showAlbum:(NSString *)imagePath photoStyle:(int)photoStyle {
    _imagePath = imagePath;
    _photoStyle = photoStyle;
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
    //UnitySendMessage("SceneTitle", "OnLoadPhoto", "");

     NSString* mediaType = [info objectForKey:UIImagePickerControllerMediaType];
    
    NSLog(@"amano %@", mediaType);
    //UIImagePickerController type =  ;
    NSLog(@"amano2 %ld", (long)UIImagePickerControllerSourceTypeCamera);
    NSLog(@"amano3 %ld", (long)UIImagePickerControllerSourceTypeSavedPhotosAlbum);
    NSLog(@"amano4 %ld", (long)UIImagePickerControllerSourceTypePhotoLibrary);
    NSLog(@"amano5 %ld", (long)picker.sourceType);


    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];

    
    NSLog(@"TEST1");
    // 画像取得
    UIImage *origin = [info objectForKey:UIImagePickerControllerOriginalImage];
    if(origin.size.width > 2048 || origin.size.height > 2048) {
            origin = [self resizedImage:origin size:CGSizeMake(origin.size.width / 2, origin.size.height / 2)];
    }
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
    NSLog(@"TEST2");

    // Unity側に情報を返す
    //_mstSelectedImage
    UnitySendMessage("MarkerButton", "onCallBack", [self parseStr:_imagePath.UTF8String ]);

    // 破棄
    [self releaseImagePicker];
    NSLog(@"TEST3  path : %s", _imagePath);
}
// - (char *)MakeStringCopy: (const char*) string{
//     if (string == NULL)
//         return NULL;
    
//     char* res = (char*)malloc(strlen(string) + 1);
//     strcpy(res, string);
//     return res;
// }


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
 * 画像クリップ
 * @param image UIImage
 * @param rect クリップ領域
 * @return UIImage
 */
- (UIImage *)clippedImage:(UIImage *)image rect:(CGRect)rect {
    rect = CGRectApplyAffineTransform(rect, [self transformForOrientation:image]);
    CGImageRef cgImage = CGImageCreateWithImageInRect(image.CGImage, rect);
    image = [UIImage imageWithCGImage:cgImage scale:image.scale orientation:image.imageOrientation];
    CGImageRelease(cgImage);

    return image;
}

/**
 * Affine変換
 * @param image UIImage
 * @return CGAffineTransform
 */
- (CGAffineTransform)transformForOrientation:(UIImage *)image {
    CGAffineTransform t = CGAffineTransformIdentity;
    CGSize visibleImageSize = image.size;
    CGSize originalImageSize = image.size;
    switch (image.imageOrientation) {
        case UIImageOrientationLeft:
        case UIImageOrientationLeftMirrored:
        case UIImageOrientationRight:
        case UIImageOrientationRightMirrored:
            originalImageSize = CGSizeMake(visibleImageSize.height, visibleImageSize.width);
            break;
        default:
            originalImageSize = visibleImageSize;
    }

    t = CGAffineTransformTranslate(t, originalImageSize.width / 2, originalImageSize.height / 2);
    switch (image.imageOrientation) {
        case UIImageOrientationDownMirrored:
            t = CGAffineTransformScale(t, -1, 1);
        case UIImageOrientationDown:
            t = CGAffineTransformRotate(t, M_PI);
            break;
        case UIImageOrientationLeftMirrored:
            t = CGAffineTransformScale(t, -1, 1);
        case UIImageOrientationLeft:
            t = CGAffineTransformRotate(t, M_PI/2);
            break;
        case UIImageOrientationRightMirrored:
            t = CGAffineTransformScale(t, -1, 1);
        case UIImageOrientationRight:
            t = CGAffineTransformRotate(t, -M_PI/2);
            break;
        case UIImageOrientationUpMirrored:
            t = CGAffineTransformScale(t, -1, 1);
        case UIImageOrientationUp:
        default:
            break;
    }
    t = CGAffineTransformTranslate(t, -visibleImageSize.width / 2, -visibleImageSize.height / 2);
    return t;
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
    NSLog(@"TEST");
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
- (void)alertTest:(NSString *)imagePath photoStyle:(int)photoStyle {
    _imagePath = imagePath;
    _photoStyle = photoStyle;
 
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
            [self showCamera:_imagePath photoStyle:_photoStyle];
        break;
        case 2:
            [self showAlbum:_imagePath photoStyle:_photoStyle];
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
    static CharikitaPlugin *plugin =[[CharikitaPlugin alloc] init];
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

    void showCamera(char *imagePath, int photoStyle){
        [plugin showCamera:getStr(imagePath) photoStyle:photoStyle];
    }

    void showAlbum(char *imagePath, int photoStyle){
        [plugin showAlbum:getStr(imagePath) photoStyle:photoStyle];
    }

    void savePhoto(char *path){
        [plugin savePhoto:getStr(path)];
    }

    void alertTest(char *imagePath, int photoStyle){
        [plugin alertTest:getStr(imagePath) photoStyle:photoStyle];
    };

    int sampleMethod1();

}
int sampleMethod1() {
    return 10;
}















