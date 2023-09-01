#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.UI;
using FTOptix.Retentivity;
using FTOptix.NativeUI;
using FTOptix.Core;
using FTOptix.CoreBase;
using FTOptix.NetLogic;
using Google.Authenticator;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Web;
using static System.Runtime.CompilerServices.RuntimeHelpers;
#endregion

public class OTP_GenerateAndValidate : BaseNetLogic
{
    private PeriodicTask myPeriodicTask;
    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
    string authKey;

    public override void Start()
    {
        // Generate session KEY
        // Please note that for real field application, this key should be stored into a file
        // and reloaded at every startup. If the key changes, the setup procedure must be done again
        authKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
    [ExportMethod]
    public void AddDevice(NodeId imgContainer, NodeId lbManualCode) {
        // Generate QR image
        SetupCode setupInfo = tfa.GenerateSetupCode("FTOptix Application TwoFactorAuth Test", "l.bennati@asem.it", authKey, false, 3);
        // Store image and manual setup script
        string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
        string manualEntrySetupCode = setupInfo.ManualEntryKey;
        // Log path of the output images
        Log.Debug("Image URL: " + qrCodeImageUrl);
        Log.Debug("Manual string: " + manualEntrySetupCode);
        Log.Info("Setup image generated");
        // Get path to PNG image
        var prjUri = new Uri(new Uri(System.IO.Directory.GetCurrentDirectory()), Project.Current.ProjectDirectory);
        var imgUri = new Uri(prjUri, Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + ".png");
        string filePath = imgUri.AbsolutePath;
        filePath = HttpUtility.UrlDecode(filePath);
        Log.Debug("File path: " + filePath);
        // Write BASE64 image to file
        string base64Image = qrCodeImageUrl.Split(",")[qrCodeImageUrl.Split(",").Length - 1];
        File.WriteAllBytes(filePath, Convert.FromBase64String(base64Image));
        // Update image file
        InformationModel.Get<FTOptix.UI.Image>(imgContainer).Path = filePath;
        InformationModel.Get<FTOptix.UI.Label>(lbManualCode).Text = manualEntrySetupCode;
    }

    [ExportMethod]
    public void ValidateCode(string code, out bool result) {
        // Validate input code ad return result as bool
        TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        result = tfa.ValidateTwoFactorPIN(authKey, code);
    }
}
