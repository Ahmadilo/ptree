# AGENTS.md

## What this is

`ptree` is a Windows .NET 8 CLI (`ptree show ...`) that renders a directory tree with focus/collapse/ignore controls, built to shape project context for LLMs and docs. See `README.md` for the full option catalog.

## Projects

- `ptree/` — the product. Console app targeting `net8.0`, deps: System.CommandLine 2.0.0 (GA API), TextCopy (clipboard). `ptree.csproj` sets Native AOT / self-contained / win-x64 unconditionally, so every `dotnet publish` produces a native win-x64 exe — keep code AOT-compatible.
- `executble/` (misspelling is intentional; keep the name) — manual smoke-test harness, not part of the product. Its `Program.cs` launches `ptree.exe` from `ptree/bin/Debug/net8.0/` against hardcoded absolute paths; edit those paths before using it.

## Commands

```powershell
dotnet build ptree.sln                       # one pre-existing CS0168 warning in Tree.cs is known
dotnet run --project ptree -- show --deep 1  # CLI args go after the --
dotnet publish ptree                         # release build: AOT single-file exe
```

There is no test project, lint, or format config. Verify changes by building and running `show` variants (e.g. `--focus`, `--collapse`, `--count-all --no-files`) against a sample directory — the repo root works.

## Architecture

All state flows through statics — there are no DI or parameters between layers:

1. `ptree/Program.cs` builds the root command via `Options.GetRootCommand()` and invokes it.
2. The `show` command's action copies parsed values into public static fields on `Options`, then calls `PrintTree.Run()`.
3. `PrintTree.Run()`: `Tree.Scan(path, deep, ignore)` → `root.Focus(focus)` → `root.Collapse(collapse)` → recursive print to console while accumulating the same text into a `StringBuilder` → optional `--copy` (clipboard) then `--log <file>` write.

To add a new option you must touch three places in `Options.cs`: define the `Option`, `showCommand.Add(...)` it, and assign it inside the `Parser` lambda. Consume it elsewhere via the static field.

## Gotchas

- Default ignored dirs (`node_modules`, `vendor`, `.git`, `bin`, `obj`) live in `Program.IgnoreDirs`; `--no-ignore` bypasses them.
- `--ignore` / `--focus` / `--collapse` match bare directory names case-insensitively, not paths.
- `Console.OutputEncoding` is forced to UTF-8 in `Main`; box-drawing characters depend on it.
- Code comments are largely in Arabic — match the existing style of the file you edit.
- `ptree-debug.json` and `ptree.txt` at the repo root are captured sample outputs (debug artifact / example), not configuration. Don't treat them as inputs.
- Version lives in `ptree.csproj` (`Version` + `InformationalVersion`).

## Git

Two remotes: `origin` (Ahmadilo/ptree) and `official` (ahmadaden/ptree). Default branch: `master`.
