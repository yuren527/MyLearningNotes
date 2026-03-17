# Perforce (P4) Command Cheat Sheet — Practical Workflow

## 🧠 Core Concept

Perforce only tracks files that are **opened**.

* If a file is not opened → P4 ignores it
* Deleting locally ≠ deleting in depot
* Always verify with `p4 opened` before submit

---

# 📦 1. Reconcile (Detect Changes Automatically)

## Detect all changes (most important command)

```bash
p4 reconcile //...
```

* Finds:

  * modified files → mark as `edit`
  * new files → mark as `add`
  * deleted files → mark as `delete`
* 🔴 REQUIRED if you changed files outside P4

---

## Explicit reconcile options

```bash
p4 reconcile -e -a -d //...
```

* `-e` → detect edits
* `-a` → detect new files
* `-d` → detect deleted files

---

## Preview reconcile (safe mode)

```bash
p4 reconcile -n //...
```

* Shows what will happen without applying changes

---

# 📂 2. Opened Files (What P4 Actually Tracks)

## Check current workspace

```bash
p4 opened
```

* Shows all files currently opened (edit/add/delete)

---

## Check specific changelist

```bash
p4 opened -c <changelist_number>
```

---

## Check ALL clients (very important)

```bash
p4 opened -a
```

* Shows files opened across all machines/workspaces

---

## Check specific client (workspace)

```bash
p4 opened -C <client_name>
```

---

## Check by user

```bash
p4 opened -u <username>
```

---

## Filter (example: only deleted files)

```bash
p4 opened -a | findstr delete
```

---

# 🗑️ 3. Deleting Files Properly

## Correct way (recommended)

```bash
p4 delete path/to/file
```

* Marks file for deletion in changelist

---

## Alternative workflow

```bash
# 1. Delete file in Explorer
# 2. Then run:
p4 reconcile //...
```

---

# 📤 4. Submit Changes

## Submit specific changelist

```bash
p4 submit -c <changelist_number>
```

---

## Default changelist submit

```bash
p4 submit
```

---

# 🔍 5. Verify After Submit

## View recent changes

```bash
p4 changes -m 5
```

---

## Inspect a changelist

```bash
p4 describe <changelist_number>
```

* Shows:

  * files included
  * whether deletes were submitted

---

## View pending changelists

```bash
p4 changes -s pending
```

---

# 🧹 6. Workspace Cleanup (Fix Mismatches)

## Clean workspace (match depot exactly)

```bash
p4 clean
```

* Removes extra files
* Restores missing files

---

## Force full resync (strong reset)

```bash
p4 sync -f //...
```

---

## Clean + sync (full fix)

```bash
p4 clean //...
p4 sync //...
```

---

# 🔄 7. Sync (Update Workspace)

## Normal sync

```bash
p4 sync
```

---

## Sync specific path

```bash
p4 sync //depot/path/...
```

---

# ⚠️ 8. Common Problems & Fixes

## Problem: Deleted file still exists in depot

**Cause:**

* File deleted locally but not marked in P4

**Fix:**

```bash
p4 reconcile //...
p4 submit
```

---

## Problem: File deleted in depot but still exists locally

**Cause:**

* Workspace not updated

**Fix:**

```bash
p4 clean
```

---

## Problem: File locked or stuck from another machine

**Check:**

```bash
p4 opened -a
```

---

# ✅ 9. Safe Daily Workflow (Recommended)

## Minimal safe workflow

```bash
p4 reconcile //...
p4 opened
p4 submit
```

---

## Safer version (multi-machine/dev team)

```bash
p4 opened -a
p4 reconcile //...
p4 opened
p4 submit
```

---

## Debug workflow (when something feels wrong)

```bash
p4 opened -a
p4 changes -s pending
p4 reconcile -n //...
```

---

# 🧠 Key Takeaways

* Always run `p4 reconcile` before submit
* Always check `p4 opened`
* Deleting locally is NOT enough
* Multi-machine = always check `p4 opened -a`

---

# 🚀 Optional Aliases (Power Users)

You can simplify your workflow:

```bash
alias p4check="p4 opened -a && p4 changes -s pending"
alias p4safe="p4 reconcile //... && p4 opened"
```

---
