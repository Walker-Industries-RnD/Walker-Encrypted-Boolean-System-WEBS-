
# Walker Encrypted Boolean System (WEBS)

###### The Walker Encrypted Boolean System is a system focused on providing high quality boolean encryption. It is made to work with C# (Particularly Unity 3D) and uses a custom system to ensure high quality protection against the most common worries of encrypting bools. This was made for the WI Pariah Cybersecurity System and XRUIOS, Coming Soon!



[![Support me on Patreon](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Fshieldsio-patreon.vercel.app%2Fapi%3Fusername%3Dwalkerdev%26type%3Dpledges&style=for-the-badge)](https://patreon.com/walkerdev)


[Note: This Requires UnityCrypto To Work](https://github.com/dubit/unity-crypto)

[Note: Many Thanks For This Nice RNG](https://gist.github.com/sachintha81/a4613d09de6b5f9d6a1a99dbf46e2385)



## Features

- Anti CopyPaste: WEBS save data is reliant on an encrypted true and false string (Done With AES Encryption) rather than a boolean as a string, meaning you can not simply copy/paste a value.

- Great Obfuscation: Alongside storing the order of the item and the true/false string, there is also a dummy string created to help minimize the chance of two values looking similar.

- Constantly Changing: To help ensure values can not be copy/pasted between changes to values, the methods provided changes all values with a new true/false string and UUID values with each boolean change.

- Currently Working on v2 with a Scrypt function for more reliable value checking, AES256 should still be generally fine 



## How Does WEBS Work

### Technical Overview: Encryption

Let's say you have a bool set to true and you want to encrypt it.

First, we generate a value to represent true and another to represent false. Then, we use AES to encrypt the value. However, we get the value representing "True" and add a dummy string to it. We use // to seperate the next value (The Bool Place in the List) and the dummy string length. For our password, we use the user given string with the order of the bool at the end.

We are given an EncryptedBoolGroup.

### Technical Overview: Decryption

Then, when we want to get our value again, we will take our EncryptedBoolGroup, find the value we want and split the string by //.

The first result is the bool value and dummy string, the second is the counter and the third is the dummy string length as an integer.

If the counter is not equal to the proper place in the boolean order, we know this was tampered with.

Now, we erase the dummy string thanks to us knowing the dummy string length, which leaves us with the bool value. If it is not equal to the true or false value presented by the EncryptedBoolGroup, we know someone copy/pasted another value.



## WEBS C# Functions

### EncryptedBoolGroup
A group of encrypted bools, this can be a singular  bool or 1000! Names are also within a separate list, although in the near future I should change this to a dictionary. The constructor will automatically encrypt any Bool Group!

### BoolGroup
A group of bools decrypted. The constructor will automatically take a bool list and string list, turning it into a bool group. It is also used by several functions to expose the true and false values.

### DecryptBoolGroup
This function handles the decryption of an EncryptedBoolGroup!

### EditBoolNameInGroup
Takes a name from a BoolGroup and changes it.

### AddBoolToGroup
Adds a bool to the end of the encryption group.

### RemoveBoolNameInGroup
Finds a bool with a specific name and removes it from the group.

### EditBoolInGroup
Changes the value of a bool with a specific name in the group.






## License

[Apache 2](https://www.apache.org/licenses/LICENSE-2.0)


