# TODO - ptree v2.0.0

> Goal: Improve the developer experience based on real-world usage over the last six months.

---

# ✅ Completed

## Clipboard Support

* [x] Add `--copy`
* [x] Integrate `TextCopy`
* [x] Copy rendered output to Clipboard
* [x] Keep Console output unchanged

---

# 🚧 Refactor Project Architecture

## Documentation

* [ ] Create `docs/architecture.md`
* [ ] Document the Pipeline
* [ ] Explain the responsibility of each stage
* [ ] Document the `show` command flow
* [ ] Map every option to its execution stage

## Project Structure

* [ ] Create `Commands/`
* [ ] Create `Pipeline/`
* [ ] Create `Scanner/`
* [ ] Create `Processing/`
* [ ] Create `Rendering/`
* [ ] Create `Outputs/`
* [ ] Create `Models/`

## Refactor Existing Code

* [ ] Move `ShowCommand` into `Commands`
* [ ] Extract `ShowPipeline`
* [ ] Move scanning logic into `TreeScanner`
* [ ] Separate rendering from outputs
* [ ] Rename files/classes to better match responsibilities

---

# 🚧 Respect .gitignore

## Scanner

* [ ] Add a `.gitignore` parser library
* [ ] Enable `.gitignore` support by default
* [ ] Add `--no-gitignore`
* [ ] Merge `.gitignore` rules with the default ignore list
* [ ] Merge `.gitignore` rules with `--ignore`

## Tests

* [ ] Test nested `.gitignore`
* [ ] Test wildcard patterns
* [ ] Test ignored directories
* [ ] Verify scanning performance

---

# 🚧 Update Command

## Command

* [ ] Implement `ptree update <file>`
* [ ] Implement `ptree update --all`

## Behavior

* [ ] Read the stored command from a log file
* [ ] Re-execute the command
* [ ] Replace the file contents
* [ ] Handle invalid commands
* [ ] Handle missing paths
* [ ] Handle malformed log files

---

# 🚧 Performance

## File Counting

* [ ] Improve counting performance
* [ ] Add count limit
* [ ] Display `1000+ Files` when the limit is reached
* [ ] Avoid spending excessive time on huge directories

---

# 🚧 Documentation

* [ ] Update README
* [ ] Add examples for the new features
* [ ] Document `--copy`
* [ ] Document `.gitignore` support
* [ ] Document `update`

---

# 🚀 Release

* [ ] Change version to **2.0.0**
* [ ] Build
* [ ] Publish
* [ ] Write Release Notes
* [ ] Publish GitHub Release