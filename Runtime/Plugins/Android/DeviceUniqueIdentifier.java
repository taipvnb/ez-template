package com.unimob.deviceuniqueidentifier;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.content.Context;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.provider.Settings;
import android.telephony.TelephonyManager;
import android.util.Log;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

public class DeviceUniqueIdentifier {

    @SuppressLint("HardwareIds")
    public static String DeviceUniqueId(Activity currentActivity) {
        String deviceIdShort = "35" +
                (Build.BOARD.length() % 10) +
                (Build.BRAND.length() % 10) +
                (Build.CPU_ABI.length() % 10) +
                (Build.DEVICE.length() % 10) +
                (Build.DISPLAY.length() % 10) +
                (Build.HOST.length() % 10) +
                (Build.ID.length() % 10) +
                (Build.MANUFACTURER.length() % 10) +
                (Build.MODEL.length() % 10) +
                (Build.PRODUCT.length() % 10) +
                (Build.TAGS.length() % 10) +
                (Build.TYPE.length() % 10) +
                (Build.USER.length() % 10);

        String androidID = Settings.Secure.getString(currentActivity.getContentResolver(), "android_id");

        String wlanMac = "";
        try {
            WifiManager wifiManager = (WifiManager) currentActivity.getApplicationContext().getSystemService(Context.WIFI_SERVICE);
            wlanMac = wifiManager.getConnectionInfo().getMacAddress();
        } catch (Exception e) {
            Log.e("Unity", "ACCESS_WIFI_STATE permission required.");
        }

        String longId = deviceIdShort + androidID + wlanMac;

        MessageDigest messageDigest = null;
        try {
            messageDigest = MessageDigest.getInstance("MD5");
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        }

        messageDigest.update(longId.getBytes(), 0, longId.length());
        byte[] md5Data = messageDigest.digest();

        String uniqueID = new String();
        for (int i = 0; i < md5Data.length; i++) {
            int b = 0xFF & md5Data[i];
            if (b <= 15) {
                uniqueID = String.valueOf(uniqueID) + "0";
            }
            uniqueID = String.valueOf(uniqueID) + Integer.toHexString(b);
        }
        return uniqueID;
    }
}