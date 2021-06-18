## Data Config Generator
###### Addon tool

### Installation

If you need to create DataConfig assets automatically, you can use this tool.
Here are few steps to set up workspace:
1. Create Google SpreadSheet document like in the link below:
https://docs.google.com/spreadsheets/d/1L77AUU-SmKj3dVjPl7hEbnWeGkr6HcT5tt0Rxgxswoo/edit#gid=0
First row and column would be your document version. You can automatically increase version by the following code: (Copy-paste it in your google-docs scripts in your document)
```js
function onEdit(cell) {
  incrementCell(SpreadsheetApp.getActive().getActiveSheet().getName(), "A1:A1");
}

function incrementCell(sheet, cell) {
  var sheet = SpreadsheetApp.getActive().getSheetByName(sheet);
  var range = sheet.getRange(cell);

  range.setValue(range.getValue() + 1);
}
```
2. Publish your document to get CSV link.
3. Create "Create/ME.ECS/Addons/Data Config Generator Settings" asset and fill up the form with the target directory, caption and csv link to your google-spreadsheet.
4. Press "Update All" and wait while operation has been complete.
5. Your data configs should automatically created in your destination directory.

### Google Spreadsheet data

##### Table definition:
First row - component names.<br>
Second row - component fields.<br>
First column - your comment.<br>
Second column - template(s) to be use.<br>
Third column - Config name.<br>

> Note, if you leave spreadsheet cell empty, there are no component would be created for this data config.

##### Default Parsers
Primitive types like int, float, etc. have built-in parsers.<br>
Array types could be added with json-format: [1, 2, 3] or ["s1", "s2", "s3"]<br>
You can use custom links to the few UnityEngine.Object types: ECS View, UnityEngine.GameObject, UnityEngine.Component and DataConfig. To use them, define it by the following code:<br>
```
config://YOUR_CONFIG_NAME
```
```
view://Assets/path/to/your/view.prefab
```
```
go://Assets/path/to/your/gameObject.prefab
```
```
component://Assets/path/to/your/gameObject/with/component.prefab
```

### Custom Parsers

If you need to parse some custom data, you can write your own implementation, juse define struct with interface ```IParser```:
```csharp
public struct Vector2IntParser : IParser {

    public bool IsValid(System.Type fieldType) {
        return typeof(Vector2Int) == fieldType;
    }

    public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {

        var prs = data.Split(';');
        result = new Vector2Int(int.Parse(prs[0]), int.Parse(prs[1]));
        return true;

    }

}
```
