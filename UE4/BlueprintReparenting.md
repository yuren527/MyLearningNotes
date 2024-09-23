# Viable ways of reparenting blueprints after breaking parent reference(Which makes blueprint unable to be opened)
## Using CoreRedirects in `DefaultEngine.ini`
```
[CoreRedirects]
+CLassRedirects=(OldName="/Script/IntelliBlueForce.DeploymentMainWidget",NewName="/Script/UserInterface.DeploymentMainWidget")
+CLassRedirects=(OldName="/Script/IntelliBlueForce.FactionManagementMainWidget",NewName="/Script/UserInterface.FactionManagementMainWidget")
+CLassRedirects=(OldName="/Script/IntelliBlueForce.UnitListItem_Deployment",NewName="/Script/UserInterface.UnitListItem_Deployment")
```

## Use AssetEditorUtilities
- Right-click in the content browser and select "Editor Utilities" > "Editor Utility Blueprint".
- Select "Asset action utility" from the parent class menu
- ![image](https://github.com/user-attachments/assets/103b966e-3b5a-4469-ae1d-4152d1b6d313)
- Don't forget to ckeck the box "Call in editor" of the custom event
