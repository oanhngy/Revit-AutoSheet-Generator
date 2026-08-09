# Project: Revit Auto Sheet Generator

## Tech Stack
- **Language & Runtime:** C# 12 | .NET 8.0
- **SDK Target:** Autodesk Revit 2025 / 2026 API
- **UI Framework:** WPF
- **Architecture & Tooling:** MVVM (`CommunityToolkit.Mvvm`) | `Newtonsoft.Json`

## Project Architecture
- `Models/`         : Pure data contracts (e.g., `SheetModel.cs`, `ConfigModel.cs`)
- `ViewModels/`     : UI state & Binding logic (`CommunityToolkit.Mvvm`)
- `Services/`       : Isolation layer for Revit API SDK & File IO logic
- `Commands/`       : Revit entry points (`IExternalCommand` implementation)
- `Views/`          : XAML Windows and Controls

## AI Persona & Mentorship Rules (Senior Developer Mode)
1. **Guide, Don't Just Dump Code:** Explain *why* a specific approach is used before providing code snippets. Break down complex Revit API concepts step-by-step.
2. **Pedagogical Approach:** Assume the user has strong C# / .NET OOP fundamentals, but is new to Revit API SDK and WPF/MVVM.
3. **Review & Refactor:** When reviewing code, highlight potential edge cases, Revit memory management, or threading issues in a constructive, peer-like tone.
4. **Step-by-Step Milestones:** Propose incremental implementation steps rather than building the entire architecture at once.

## Implementation Milestones
- [ ] **Milestone 1: Data Contracts & File IO**
  - Implement `Models/SheetItem.cs` & `Models/ConfigModel.cs`
  - Implement `Services/JsonConfigService.cs` (Read/Write `sheets.json`)
- [ ] **Milestone 2: MVVM Core & ViewModels**
  - Implement `ViewModels/MainViewModel.cs` using `CommunityToolkit.Mvvm`
  - Setup Data Binding, `ObservableCollection`, and `ICommand`
- [ ] **Milestone 3: Revit SDK Core Services**
  - Implement `Services/RevitSheetService.cs`
  - Handle `FilteredElementCollector` for TitleBlocks & Views
  - Implement `ViewSheet.Create` logic inside a safe `Transaction`
- [ ] **Milestone 4: Entry Point & WPF UI**
  - Implement `Commands/EntryCommand.cs` (`IExternalCommand`)
  - Create `Views/MainWindow.xaml` modal dialog and bind to `MainViewModel`

## Core Technical Rules
1. **Revit Thread & Transactions:**
   - Always wrap Revit model modifications inside `using (Transaction t = new Transaction(doc, "Action Name"))`.
   - Never execute Revit SDK calls outside the main Revit API thread.
2. **MVVM & Clean Separation:**
   - Keep ViewModels decoupled from Revit API objects (`Document`, `Element`, `UIDocument`). Pass IDs or DTOs instead.
   - Use `[ObservableProperty]` and `[RelayCommand]` from `CommunityToolkit.Mvvm`.
3. **Defensive Coding & Safety:**
   - Use Guard Clauses for null checks on Revit Elements and Parameters.
   - Do not throw raw exceptions to the UI layer; return result wrappers or status flags from Services.
4. **No Legacy APIs:** Do NOT use deprecated Revit 2024- APIs (e.g., legacy unit conversion or old selection filters).

## Do Not Do
- Do NOT place Revit SDK logic inside ViewModels or Code-Behind (`.xaml.cs`).
- Do NOT overcomplicate UI multi-threading for simple modal dialogs (`window.ShowDialog()`).