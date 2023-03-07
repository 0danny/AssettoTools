# AssettoTools

Soon to be Assetto Corsa car modifier.

Will be able to modifiy all INI files from a desktop application.

Comes with in-house ACD extractor and decryptor, custom wrote. Please see ACDBackend project.

Text editor is AvalonEdit, custom made syntax highlighting so it may not be perfect. Still working on it.

As of right now if you want to use, you will need to build it yourself.

## Pictures

![image](https://user-images.githubusercontent.com/14921414/223351743-99046f84-5e5b-47a3-9d1d-e34d894ba87d.png)

## TODO

- ~~Add a save button to re-encrypt ACD.~~
- ~~Add an area to set your Assetto path (Settings tab?)~~
- Dark/Light mode switch.
- Fix saving for cars that don't have a ACD & just data folder.
- Add a menu for less advanced users 
     - Lets people edit simple things like turbo PSI, wastegate pressure etc.
     - Good if they are unsure on how to edit the .INI files.
- Add syntax highlighting for the .LUT files.
     - It is kind of broken with brackets (look at preview nodes)

# ACD Extractor

Below is an explanation on how the CreateKey & Decryption functions work. Assetto utilises a simple ROT algorithim for the encryption/decryption.

## Helper Functions

### Int To Byte

```csharp
(byte)((value % 256 + 256) % 256);
```

## CreateKey()

### Params
* string - folderName

Assetto uses the folder name of the car to create an encryption/decryption key. Once the .acd is packed with the key, if the folder name is changed it will no longer be decryptable. In the example below the folder name we are using is:

#### ks_audi_sport_quattro

### Key Structure

The key is structured as follows:

000-000-000-000-000-000-000-000

Each octet is concatenated with a "-" delimiter.

- - - -

### Octet 1

```csharp
     folderName = folderName.ToLower();

     int aggregateSeed = folderName.Aggregate(0, (int current, char t) =>
     {
        return current + t;
     });
```

Firstly the folderName is converted to lower case and its characters represented as its unicode counterpart are summed together, this gives:

#### 2278

The process of aggregation is shown below.

```
Current: 0, T: k, T(NUM): 107
Current: 107, T: s, T(NUM): 115
Current: 222, T: _, T(NUM): 95
Current: 317, T: a, T(NUM): 97
Current: 414, T: u, T(NUM): 117
Current: 531, T: d, T(NUM): 100
Current: 631, T: i, T(NUM): 105
Current: 736, T: _, T(NUM): 95
Current: 831, T: s, T(NUM): 115
Current: 946, T: p, T(NUM): 112
Current: 1058, T: o, T(NUM): 111
Current: 1169, T: r, T(NUM): 114
Current: 1283, T: t, T(NUM): 116
Current: 1399, T: _, T(NUM): 95
Current: 1494, T: q, T(NUM): 113
Current: 1607, T: u, T(NUM): 117
Current: 1724, T: a, T(NUM): 97
Current: 1821, T: t, T(NUM): 116
Current: 1937, T: t, T(NUM): 116
Current: 2053, T: r, T(NUM): 114
Current: 2167, T: o, T(NUM): 111
```

The result of **2278** is then converted to a single byte represention using the helper function **intToByte** shown above which yields:

#### 230

This is the first octet of the key.

- - - -

### Global Variables

**a** = folderName Length

**b** = folderName Array

### Octet 2

For convenience the rest of the calculations will be represented as a math/programming expression. Treat summation as a simple loop. 

**temp** = 0

![image](https://user-images.githubusercontent.com/14921414/221389749-0ce35a60-31e8-4b0d-9631-a7502f75cee2.png)

### Octet 3

**temp** = 0

![image](https://user-images.githubusercontent.com/14921414/221390075-4c56be5d-6853-46de-8f6a-79c0709e4bf3.png)

### Octet 4

**temp** = 5763

![image](https://user-images.githubusercontent.com/14921414/221390137-94252822-1b4e-4061-aaa4-c3abef101e92.png)

### Octet 5

**temp** = 66

![image](https://user-images.githubusercontent.com/14921414/221390203-578ebb97-838b-4f32-99a6-b511aa953645.png)

### Octet 6

**temp** = 101

![image](https://user-images.githubusercontent.com/14921414/221390239-c540ca43-6b5e-438c-958f-d741033a569f.png)

### Octet 7

**temp** = 171

![image](https://user-images.githubusercontent.com/14921414/221390475-ebf40b09-8415-4741-9a27-a1e972877913.png)

### Octet 8

**temp** = 171

![image](https://user-images.githubusercontent.com/14921414/221390530-113a9c61-3059-4da2-8501-09171c6d7c61.png)

### Touches

All octets are ran through the IntToByte function and their product is concatenated using a "-" delimiter as seen below.

```csharp
key = string.Join("-", new object[]
{
  octet1,
  octet2,
  octet3,
  octet4,
  octet5,
  octet6,
  octet7,
  octet8
});
```

I understand the above probably looks like gibberish and it's most likely easier to just read the code, however I thought I might aswell include it anyways.

## Decrypt()

### Params
* byte[] - array

The decrypt & encrypt functions both make use of the key created above to decrypt the values within the ACD file.

## Encrypt()

### Params
* byte[] - data
* byte[] - result (pointer)
