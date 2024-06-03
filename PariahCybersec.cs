using System;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using DUCK.Crypto;

public class PariahCybersec: MonoBehaviour {
  internal static string GlobalPass;

  public class EncryptedBoolGenerator {
    public struct EncryptedBoolGroup {
      internal List < SimpleAESEncryption.AESEncryptedText > BoolValues;
      internal List < SimpleAESEncryption.AESEncryptedText > BoolNames;
      internal SimpleAESEncryption.AESEncryptedText Trueval;
      internal SimpleAESEncryption.AESEncryptedText Falseval;

      public EncryptedBoolGroup(BoolGroup boolgroupitem) {
        Utilities utilistance = new Utilities();
        string trueval = utilistance.CreateUUID();
        string falseval = utilistance.CreateUUID();

        BoolValues = new List < SimpleAESEncryption.AESEncryptedText > ();
        BoolNames = new List < SimpleAESEncryption.AESEncryptedText > ();

        int i = 0;

        foreach(bool item in boolgroupitem.BoolValues) {

          var dummystring = utilistance.GetRandomLengthString();

          if (item == true) {
            BoolValues.Add(SimpleAESEncryption.Encrypt(trueval + dummystring + "//" + i + "//" + dummystring.Length, GlobalPass + i));
          } else if (item == false) {
            BoolValues.Add(SimpleAESEncryption.Encrypt(falseval + dummystring + "//" + i + "//" + dummystring.Length, GlobalPass + i));
          }

          i++;
        }

        foreach(string item in boolgroupitem.BoolNames) {
          BoolNames.Add(SimpleAESEncryption.Encrypt(item, GlobalPass));
        }

        Trueval = SimpleAESEncryption.Encrypt(trueval, GlobalPass);
        Falseval = SimpleAESEncryption.Encrypt(falseval, GlobalPass);

      }
    }

    public struct BoolGroup {
      internal List < bool > BoolValues;
      internal List < string > BoolNames;
      internal string ? TrueVal;
      internal string ? FalseVal;

      public BoolGroup(List < bool > boolValues, List < string > boolNames, string ? trueVal, string ? falseVal) {
        BoolValues = boolValues;
        BoolNames = boolNames;
        TrueVal = trueVal;
        FalseVal = falseVal;
      }
    }

    public BoolGroup ? DecryptBoolGroup(EncryptedBoolGroup encryptedgroup) {

      var trueval = SimpleAESEncryption.Decrypt(encryptedgroup.Trueval, GlobalPass);

      var falseval = SimpleAESEncryption.Decrypt(encryptedgroup.Falseval, GlobalPass);

      List < string > boolnames = new List < string > ();

      int i = 0;
      foreach(SimpleAESEncryption.AESEncryptedText item in encryptedgroup.BoolNames) {
        boolnames.Add(SimpleAESEncryption.Decrypt(item, GlobalPass + i));
      }

      List < bool > bools = new List < bool > ();

      int i2 = 0;

      foreach(SimpleAESEncryption.AESEncryptedText item in encryptedgroup.BoolValues) {

        //(trueval + dummystring + "//" + i + "//" + dummystring.Length, GlobalPass + i));

        var decryptedbool = (SimpleAESEncryption.Decrypt(item, GlobalPass + i));

        string[] parts = decryptedbool.Split(new string[] {
          "//"
        }, StringSplitOptions.None);

        string boolvalandstring = parts[0];
        int counter = int.Parse(parts[1]);
        int dummystringlengthasint = int.Parse(parts[2]);

        if (counter == i) {
          //Do nothing, continue
        } else {
          Debug.Log("The Counter Variable Does Not Match The Bool's Place, Tampered!");
          return null;
        }

        boolvalandstring = boolvalandstring.Substring(0, boolvalandstring.Length - dummystringlengthasint);

        if (boolvalandstring == trueval) {
          bools.Add(true);
        } else if (boolvalandstring == falseval) {
          bools.Add(false);
        } else {

          Debug.Log("The Bool Has Been Tampered With!");
          return null;

        }

        i2++;

      }

      var finalreturn = new BoolGroup(bools, boolnames, null, null);

      return finalreturn;

    }

    public EncryptedBoolGroup ? EditBoolNameInGroup(string Name, string NewName, bool NewVal, EncryptedBoolGroup encryptedgroup) {
      var utilistance = new Utilities();
      var trueval = utilistance.CreateUUID();
      var falseval = utilistance.CreateUUID();

      BoolGroup decryptedgroup = (BoolGroup) DecryptBoolGroup(encryptedgroup);

      int i = 0;

      bool wasvalfound = false;

      foreach(string item in decryptedgroup.BoolNames) {
        if (item == Name) {
          wasvalfound = true;
          decryptedgroup.BoolNames.RemoveAt(i);
          decryptedgroup.BoolNames.Insert(i, NewName);
          break;
        }
        i++;
      }

      if (wasvalfound == false) {
        Debug.Log("The name was not found!");
        return null;
      }

      var NewBG = new EncryptedBoolGroup(decryptedgroup);
      return NewBG;

    }

    public EncryptedBoolGroup AddBoolToGroup(string NewName, bool NewVal, EncryptedBoolGroup encryptedgroup) {
      var utilistance = new Utilities();
      var trueval = utilistance.CreateUUID();
      var falseval = utilistance.CreateUUID();

      BoolGroup decryptedgroup = (BoolGroup) DecryptBoolGroup(encryptedgroup);

      decryptedgroup.BoolNames.Add(NewName);
      decryptedgroup.BoolValues.Add(NewVal);

      var NewBG = new EncryptedBoolGroup(decryptedgroup);
      return NewBG;

    }

    public EncryptedBoolGroup ? RemoveBoolNameInGroup(string Name, bool NewVal, EncryptedBoolGroup encryptedgroup) {
      var utilistance = new Utilities();
      var trueval = utilistance.CreateUUID();
      var falseval = utilistance.CreateUUID();

      BoolGroup decryptedgroup = (BoolGroup) DecryptBoolGroup(encryptedgroup);

      int i = 0;

      bool wasvalfound = false;

      foreach(string item in decryptedgroup.BoolNames) {
        if (item == Name) {
          wasvalfound = true;
          decryptedgroup.BoolNames.RemoveAt(i);
          decryptedgroup.BoolValues.RemoveAt(i);
          break;
        }
        i++;
      }

      if (wasvalfound == false) {
        Debug.Log("The name was not found!");
        return null;
      }

      var NewBG = new EncryptedBoolGroup(decryptedgroup);
      return NewBG;

    }

    public EncryptedBoolGroup ? EditBoolInGroup(string Name, bool NewVal, EncryptedBoolGroup encryptedgroup) {
      var utilistance = new Utilities();
      var trueval = utilistance.CreateUUID();
      var falseval = utilistance.CreateUUID();

      BoolGroup decryptedgroup = (BoolGroup) DecryptBoolGroup(encryptedgroup);

      int i = 0;

      bool wasvalfound = false;

      foreach(string item in decryptedgroup.BoolNames) {
        if (item == Name) {
          wasvalfound = true;
          decryptedgroup.BoolValues.RemoveAt(i);
          decryptedgroup.BoolValues.Insert(i, NewVal);
          break;
        }
        i++;
      }

      if (wasvalfound == false) {
        Debug.Log("The name was not found!");
        return null;
      }

      var NewBG = new EncryptedBoolGroup(decryptedgroup);
      return NewBG;

    }

  }

  public class Utilities {
    internal string CreateUUID() {
      string UUID =
        default;

      int i;

      for (i = 0; i < 20;) {
        var newval = RNGCSP.RollDice(9).ToString();
        UUID = string.Concat(UUID, newval);
      }

      return UUID;

    }

    internal string GetRandomLengthString() {
      string Val =
        default;

      int turns = RNGCSP.RollDice(20);

      int i;

      for (i = 0; i < turns;) {
        var newval = RNGCSP.RollDice(9).ToString();
        Val = string.Concat(Val, newval);
      }

      return Val;
    }

  }

  //Copied from https://gist.github.com/sachintha81/a4613d09de6b5f9d6a1a99dbf46e2385

  class RNGCSP {
    private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
    // Main method.
    public static void Main() {
      const int totalRolls = 25000;
      int[] results = new int[6];

      // Roll the dice 25000 times and display
      // the results to the console.
      for (int x = 0; x < totalRolls; x++) {
        byte roll = RollDice((byte) results.Length);
        results[roll - 1]++;
      }
      for (int i = 0; i < results.Length; ++i) {
        Console.WriteLine("{0}: {1} ({2:p1})", i + 1, results[i], (double) results[i] / (double) totalRolls);
      }
      rngCsp.Dispose();
      Console.ReadLine();
    }

    // This method simulates a roll of the dice. The input parameter is the
    // number of sides of the dice.

    public static byte RollDice(byte numberSides) {
      if (numberSides <= 0)
        throw new ArgumentOutOfRangeException("numberSides");

      // Create a byte array to hold the random value.
      byte[] randomNumber = new byte[1];
      do {
        // Fill the array with a random value.
        rngCsp.GetBytes(randomNumber);
      }
      while (!IsFairRoll(randomNumber[0], numberSides));
      // Return the random number mod the number
      // of sides.  The possible values are zero-
      // based, so we add one.
      return (byte)((randomNumber[0] % numberSides) + 1);
    }

    private static bool IsFairRoll(byte roll, byte numSides) {
      // There are MaxValue / numSides full sets of numbers that can come up
      // in a single byte.  For instance, if we have a 6 sided die, there are
      // 42 full sets of 1-6 that come up.  The 43rd set is incomplete.
      int fullSetsOfValues = Byte.MaxValue / numSides;

      // If the roll is within this range of fair values, then we let it continue.
      // In the 6 sided die case, a roll between 0 and 251 is allowed.  (We use
      // < rather than <= since the = portion allows through an extra 0 value).
      // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair
      // to use.
      return roll < numSides * fullSetsOfValues;
    }
  }

}
