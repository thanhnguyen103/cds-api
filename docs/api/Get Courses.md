# Get Courses API

## Description

### Request Model

- **[C]**: Conditional field

```text
{"CourseTitle"[C][string], "CourseCode"[C][string]}
```

### Response Model

```text
{"CourseTitle"[string], "CourseCode"[string], "EffectiveDate"[datetime], "ExpiryDate"[datetime]}
```

## Acceptance Criteria

### 1. Successful API

- **1.1.** At least one conditional field is provided  
  - **Result:** Returns matching courses.

- **1.2.** At least one conditional field is provided, but some headers contain unmatched information (filter returns zero results)  
  - **Result:** Returns a blank list.

### 2. Failed API

- **2.1.** No conditional field is provided  
  - **Result:** Returns message: `Missing conditional field, must provide at least 1`.

- **2.2.** Data is in the wrong format  
  - **Result:** Returns message: `Wrong format`.
