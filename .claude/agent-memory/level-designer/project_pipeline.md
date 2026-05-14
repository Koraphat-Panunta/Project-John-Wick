---
name: project-pipeline-config
description: Render pipeline, Unity version, and editor folder structure for Project-John-Wick
metadata:
  type: project
---

## Project Pipeline and Folder Notes

- **Unity version:** Unity 6 (packages at 17.2.0)
- **Render pipelines:** Both URP (`com.unity.render-pipelines.universal 17.2.0`) AND HDRP (`com.unity.render-pipelines.high-definition 17.2.0`) are installed.
  - Default material from `BuiltinMaterials.defaultMaterial` is safe to use for blockout meshes.
  - Do not hardcode URP-specific or HDRP-specific material paths.
- **No Assets/Editor/ folder exists by default.** The Write tool can create it by writing a file directly to `Assets/Editor/<filename>` — the directory will be created automatically.
- **No assembly definitions** in the main game code folders (only in MK shader packages and Odin).
- **No existing editor base classes** to extend as of 2026-05-14.
- **ProBuilder:** `com.unity.probuilder 6.0.8`
- **Input System:** `com.unity.inputsystem 1.14.2`
- **NavMesh:** `com.unity.ai.navigation 2.0.9`

**Why:** Knowing both pipelines are installed prevents hardcoding material paths. Knowing no asmdef exists means editor scripts just need to be in an `Editor/` folder.

**How to apply:** When generating editor scripts, write directly to `Assets/Editor/<Name>.cs`. Do not assume URP or HDRP specific APIs without checking.
