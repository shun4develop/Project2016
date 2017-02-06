extern "C" {    
    
    void statusBarHidden(){
    	UIViewController* parent = UnityGetGLViewController();
        parent.view.tag = 999;
        [parent setNeedsStatusBarAppearanceUpdate];
    }
    void statusBarShow(){
    	UIViewController* parent = UnityGetGLViewController();
        parent.view.tag = 0;
        [parent setNeedsStatusBarAppearanceUpdate];
    }
}


//UnityViewControllerBaseiOS.mmに追記
// - (BOOL)prefersStatusBarHidden
// {
//     return self.view.tag == 0 ? YES:NO;//NOのとき表示する
// }