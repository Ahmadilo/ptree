# ptree 🌲
ptree is a directory tree viewer designed to give you control over how a project is seen.

Instead of dumping a chaotic file structure, ptree lets you shape the context: what to show,
what to hide, and what to emphasize. It is built for developers who need to document
structures or provide clear context to LLMs.

## Getting Started

`ptree` is designed to give you control over how a project is *seen*.

Instead of dumping the entire structure, ptree lets you **shape the context**:
what to show, what to hide, and what to emphasize.

All interactions start with a single command: `show`.

Below are common usage patterns and what each command *does conceptually*.

---

### Basic project view


#### command

```bash
ptree show
```

#### Result

```txt
foodProject
├── app.py
├── config.py
├── models/
│   ├── database.py
│   ├── models.py
│   ├── __init__.py
│   └── __pycache__/
│       ├── database.cpython-313.pyc
│       ├── models.cpython-313.pyc
│       └── __init__.cpython-313.pyc
├── requirements.txt
├── routes/
│   ├── admin_routes.py
│   ├── auth_routes.py
│   ├── delivery_routes.py
│   ├── menu_routes.py
│   ├── order_routes.py
│   └── __pycache__/
│       ├── admin_routes.cpython-313.pyc
│       ├── auth_routes.cpython-313.pyc
│       ├── delivery_routes.cpython-313.pyc
│       ├── menu_routes.cpython-313.pyc
│       └── order_routes.cpython-313.pyc
├── static/
│   ├── css/
│   │   ├── admin_menu.css
│   │   ├── card.css
│   │   ├── home.css
│   │   ├── home_hero.css
│   │   ├── login.css
│   │   ├── menu.css
│   │   ├── nav.css
│   │   ├── register.css
│   │   └── style.css
│   ├── images/
│   │   ├── 3da808a3cc43452ea3aa065a2032f828.png
│   │   ├── 8683882ab1bb4aa48d791b5ec9aeebcf.png
│   │   ├── f0d4251923d34d1fa9f68fc0474d365a.png
│   │   ├── FOOD.png
│   │   └── placeholder.png
│   └── js/
│       └── menu.js
├── templates/
│   ├── admin_edit_menu.html
│   ├── admin_edit_user.html
│   ├── admin_menu.html
│   ├── admin_orders.html
│   ├── admin_users.html
│   ├── base.html
│   ├── cart.html
│   ├── delivery_orders.html
│   ├── index.html
│   ├── login.html
│   ├── menu.html
│   ├── my_orders.html
│   ├── order_details.html
│   ├── payment_page.html
│   └── register.html
└── __pycache__/
    └── config.cpython-313.pyc
````

Scans the project directory and renders a structural snapshot
using default depth and ignore rules.

This is the fastest way to get an initial sense of the project layout.

---

### Ignoring specific directories

#### Command

```bash
ptree show --ignore __pycache__
```

#### Result

```txt
foodProject
├── app.py
├── config.py
├── models/
│   ├── database.py
│   ├── models.py
│   └── __init__.py
├── requirements.txt
├── routes/
│   ├── admin_routes.py
│   ├── auth_routes.py
│   ├── delivery_routes.py
│   ├── menu_routes.py
│   └── order_routes.py
├── static/
│   ├── css/
│   │   ├── admin_menu.css
│   │   ├── card.css
│   │   ├── home.css
│   │   ├── home_hero.css
│   │   ├── login.css
│   │   ├── menu.css
│   │   ├── nav.css
│   │   ├── register.css
│   │   └── style.css
│   ├── images/
│   │   ├── 3da808a3cc43452ea3aa065a2032f828.png
│   │   ├── 8683882ab1bb4aa48d791b5ec9aeebcf.png
│   │   ├── f0d4251923d34d1fa9f68fc0474d365a.png
│   │   ├── FOOD.png
│   │   └── placeholder.png
│   └── js/
│       └── menu.js
└── templates/
    ├── admin_edit_menu.html
    ├── admin_edit_user.html
    ├── admin_menu.html
    ├── admin_orders.html
    ├── admin_users.html
    ├── base.html
    ├── cart.html
    ├── delivery_orders.html
    ├── index.html
    ├── login.html
    ├── menu.html
    ├── my_orders.html
    ├── order_details.html
    ├── payment_page.html
    └── register.html
```

Excludes one or more directories from the project context.

Useful when temporary, generated, or noisy folders distract from
understanding the real structure.

---

### Changing the Scan Root (`--from`)

You don't need to cd into folders. You can point `ptree` to any sub-directory directly.

#### Command

```bash
ptree show --from static/css
```

#### Result

```txt
css
├── admin_menu.css
├── card.css
├── home.css
├── home_hero.css
├── login.css
├── menu.css
├── nav.css
├── register.css
└── style.css
```


### Focusing on a specific area

#### Command

```bash
ptree show --focus static
```

#### Result

```bash
ptree show --focus static
foodProject
├── app.py
├── config.py
├── models/ (collapsed)
├── requirements.txt
├── routes/ (collapsed)
├── static/
│   ├── css/
│   │   ├── admin_menu.css
│   │   ├── card.css
│   │   ├── home.css
│   │   ├── home_hero.css
│   │   ├── login.css
│   │   ├── menu.css
│   │   ├── nav.css
│   │   ├── register.css
│   │   └── style.css
│   ├── images/
│   │   ├── 3da808a3cc43452ea3aa065a2032f828.png
│   │   ├── 8683882ab1bb4aa48d791b5ec9aeebcf.png
│   │   ├── f0d4251923d34d1fa9f68fc0474d365a.png
│   │   ├── FOOD.png
│   │   └── placeholder.png
│   └── js/
│       └── menu.js
├── templates/ (collapsed)
└── __pycache__/ (collapsed)
```

Builds the entire project tree, then collapses everything
except the specified directory and its full hierarchy.

This command answers the question:

> “Show me *this part* of the project, and only what is necessary around it.”

---

### Focus first, then trim

#### Command

```bash
ptree show --focus static --collapse images
```

```txt
foodProject
├── app.py
├── config.py
├── models/ (collapsed)
├── requirements.txt
├── routes/ (collapsed)
├── static/
│   ├── css/
│   │   ├── admin_menu.css
│   │   ├── card.css
│   │   ├── home.css
│   │   ├── home_hero.css
│   │   ├── login.css
│   │   ├── menu.css
│   │   ├── nav.css
│   │   ├── register.css
│   │   └── style.css
│   ├── images/ (collapsed)
│   └── js/
│       └── menu.js
├── templates/ (collapsed)
└── __pycache__/ (collapsed)
```

First establishes context by focusing on a directory,
then selectively collapses parts you decide are irrelevant.

This allows you to:

1. Discover structure
2. Then reduce noise intentionally

---

### Structural overview without files

#### Command

```bash
ptree show --count --no-files
```

```bash
foodProject
├── models/ (6 Files)
│   └── __pycache__/ (3 Files)
├── routes/ (10 Files)
│   └── __pycache__/ (5 Files)
├── static/ (15 Files)
│   ├── css/ (9 Files)
│   ├── images/ (5 Files)
│   └── js/ (1 Files)
├── templates/ (15 Files)
└── __pycache__/ (1 Files)
```

Removes files from the view and keeps directories only,
while annotating each directory with the number of files it contains.

Ideal for:

* Understanding project weight
* Spotting complexity hotspots
* High-level architectural review

---

### Saving a reusable snapshot

#### Command

```bash
ptree show --count --no-files --log snapshot.txt
```

### Control the Level of Tree View

```txt
ptree show --deep 1
foodProject
├── app.py
├── config.py
├── models/
├── requirements.txt
├── routes/
├── snapshot.txt <- that is log file
├── static/
├── templates/
└── __pycache__/
```

Generates the same contextual view, then saves it to a file.

This snapshot can be:

* Shared with teammates
* Sent to LLMs
* Stored as documentation
* Reused without rescanning

---

## Final Thought

ptree is not about showing more.
It’s about showing *exactly enough*.

You decide the context.
You decide the focus.
You decide the noise level.

---

### Control the Context, Lead The LLMs.

**That is ptree.**