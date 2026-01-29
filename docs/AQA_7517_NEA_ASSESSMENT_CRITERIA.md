# AQA A-Level Computer Science NEA (7517) Checklist

**Total Marks:** 75  
**Weighting:** 20% of A-Level  

This document is designed to help you cross-reference your project to ensure you have included all necessary items in each section.

---

## 1. Analysis (9 Marks)

Your analysis must describe the problem area and specific problem being solved. It serves as the foundation for your entire project.

### Checklist
Ensure your report includes the following sections:
- [ ] **Research Methods:** Explain how you gathered information (Interviews, Questionnaires, Observation) and why you chose them.
- [ ] **Background / Identification of Problem:** What is the problem? Why is a computer system needed?
- [ ] **Description of Current System:** How is it done currently? What are the flaws?
- [ ] **Identification of Prospective User(s):** Who will use it? What are their skills?
- [ ] **User Needs:** Specific requirements from the user (what they want the system to do).
- [ ] **Data Sources and Destinations:** Where does data come from and go to?
- [ ] **Data Volumes:** Expected amount of data (records, transactions per second, etc.).
- [ ] **Data Dictionary:** List of variables/data stores required (Name, Type, Size, Validation).
- [ ] **Data Flow Diagrams (DFD):** Level 0 and Level 1 diagrams showing current and proposed systems.
- [ ] **Entity Relationship Diagrams (ERD):** For database projects (at least 3 tables).
- [ ] **Object Orientation Planning:** Class diagrams (if using OOP).
- [ ] **Objectives:** **Numbered** and **SMART** (Specific, Measurable, Attainable, Relevant, Time-bound) objectives.
    - [ ] System Objectives
    - [ ] Processing Objectives
    - [ ] User Objectives
- [ ] **Potential Solutions:** Comparison of at least 3 ways to solve the problem.
- [ ] **Chosen Solution:** Justification for your choice (Language, Platform, Database).

### Marking Criteria (Top Band: 7-9 Marks)
* Fully or nearly fully scoped analysis of a real problem.
* Requirements fully documented in a set of **measurable** and **appropriate** specific objectives.
* Requirements arrived at by considering, through dialogue, the needs of the intended users.
* Problem sufficiently well modelled (e.g., using DFDs, ERDs) to be of use in subsequent stages.

---

## 2. Design (12 Marks)

This section articulates how the key aspects of the solution are structured.

### Checklist
- [ ] **Overall System Design:** Input, Process, Storage, Output (IPSO) chart.
- [ ] **Modular Design:** Breakdown of the system into modules/sub-routines (Structure charts or Site maps).
- [ ] **Design Data Dictionary:** Refined data requirements including database tables or file structures.
- [ ] **Validation:** Planned validation checks (Presence, Length, Range, Type, Lookup/List).
- [ ] **Database Design:** Normalised ERD (3rd Normal Form) and relationship descriptions.
- [ ] **SQL Queries:** Planned SQL for Select, Insert, Update, and Delete.
- [ ] **Algorithms:** Pseudocode and plain English descriptions for key algorithms (6-8 complex examples).
- [ ] **Object Oriented Design:** Class diagrams detailing attributes (private/public), methods, and inheritance.
- [ ] **User Interface (HCI) Design:** Annotated sketches/wireframes of key screens (Inputs and Outputs) with rationale (why did you choose this layout/colour/control?).
- [ ] **Security and Integrity:** Measures for data protection (passwords, access levels) and data integrity (validation, referential integrity).
- [ ] **Testing Strategy:** Plan for Black-box, White-box, and System testing.

### Marking Criteria (Top Band: 10-12 Marks)
* Fully or nearly fully articulated design for a real problem.
* Describes how all or almost all of the key aspects of the solution are to be structured.
* Includes clear algorithms, data structures, and database designs.

---

## 3. Technical Solution (42 Marks)

This is the implementation of your design. Marks are split into **Completeness (15 marks)** and **Techniques (27 marks)**.

### Checklist
- [ ] **Full Code Listing:**
    - [ ] All code must be copy-pasted into the document (do not use screenshots for code).
    - [ ] Use a readable monospace font (e.g., Courier New size 9).
    - [ ] **Start each class/module on a new page**.
    - [ ] **Include the filename (e.g., `Customer.cs`) clearly at the top of each page**.
- [ ] **Module/Form Evidence:** For each form/screen/class:
    - [ ] Title/Name.
    - [ ] Annotated Screenshot of the running interface.
    - [ ] Specific code listing for that module.
- [ ] **Database Evidence:** Screenshots of table designs (Design View) and relationships.
- [ ] **Technical Skills Justification Table:**
    - [ ] A dedicated table highlighting where you have used Group A and Group B skills.
    - [ ] You must include **Page Numbers** and **Class/Method names** to help the marker find the evidence.
- [ ] **Coding Styles Justification:** Explanation of how you met 'Excellent' coding styles (referencing the table below).

### Marking Criteria (Top Band: 11-15 Marks for Completeness)
* A system that meets almost all of the requirements of a solution/investigation.
* Includes some of the most important requirements defined in the Analysis.

### Technical Skills Table (27 Marks)
To achieve the highest marks (Level 3: 19-27 marks), you must demonstrate skills primarily from **Group A**.

| Group | Model (including data model/structure) | Algorithms |
| :--- | :--- | :--- |
| **Group A**<br>*(Complex)* | • Complex data model in database (e.g. several interlinked tables)<br>• Hash tables, lists, stacks, queues, graphs, trees or structures of equivalent standard<br>• Files(s) organised for direct access<br>• Complex scientific / mathematical / robotics / control / business model<br>• Complex user-defined use of OOP model (e.g. classes, inheritance, composition, polymorphism, interfaces)<br>• Complex client-server model | • Cross-table parameterised SQL<br>• Aggregate SQL functions<br>• Graph/Tree Traversal<br>• List operations / Linked list maintenance<br>• Stack/Queue Operations<br>• Hashing<br>• Advanced matrix operations<br>• Recursive algorithms<br>• Complex user-defined algorithms (e.g. optimisation, minimisation, scheduling, pattern matching)<br>• Mergesort or similarly efficient sort<br>• Dynamic generation of objects based on complex OOP model<br>• Server-side scripting using request/response objects<br>• Calling parameterised Web service APIs and parsing JSON/XML |
| **Group B**<br>*(Intermediate)* | • Simple data model in database (e.g. two or three interlinked tables)<br>• Multi-dimensional arrays<br>• Dictionaries<br>• Records<br>• Text files / Files organised for sequential access<br>• Simple scientific / mathematical / robotics / control / business model<br>• Simple OOP model<br>• Simple client-server model | • Single table or non-parameterised SQL<br>• Bubble sort<br>• Binary search<br>• Writing and reading from files<br>• Simple user defined algorithms (e.g. range of mathematical/statistical calculations)<br>• Generation of objects based on simple OOP model<br>• Calling Web service APIs and parsing JSON/XML |
| **Group C**<br>*(Basic)* | • Single-dimensional arrays<br>• Appropriate choice of simple data types<br>• Single table database | • Linear search<br>• Simple mathematical calculations (e.g. average)<br>• Non-SQL table access |

### Coding Styles Table
To maximize marks in the Technical Solution, your code must adhere to the "Excellent" standard.

| Style | Characteristic |
| :--- | :--- |
| **Excellent** | • **Modules (subroutines) with appropriate interfaces.**<br>• **Loosely coupled modules:** Module code interacts with other parts of the program through its interface only (no reliance on global states).<br>• **Cohesive modules:** Module code does just one thing.<br>• **Grouped Modules:** Subroutines with common purposes are grouped (e.g., Classes or Library files).<br>• **Defensive programming:** Code handles unexpected inputs gracefully (e.g. SQL Injection prevention).<br>• **Good exception handling:** Use of Try/Catch blocks. |
| **Good** | • Well-designed user interface.<br>• Modularisation of code.<br>• Good use of local variables (Minimal use of global variables).<br>• Managed casting of types.<br>• Use of Constants.<br>• Appropriate indentation.<br>• Self-documenting code (Variable names explain their purpose).<br>• Consistent style throughout.<br>• File paths parameterised (not hard-coded). |
| **Basic** | • Meaningful identifier names.<br>• Annotation (comments) used effectively where required. |

---

## 4. Testing (8 Marks)

Evidence that the system works and meets the requirements. Testing must include **video evidence** to support your screenshots.

### Video Requirements
You must submit actual video files (do not upload to YouTube/Vimeo).
- [ ] **Video 1: Completeness / Whole System:** A walkthrough showing the full system working to meet the main objectives.
- [ ] **Video 2: Specific Features:** Focused testing of specific complex algorithms, data structures, and validation.
- [ ] **Narration:** Both videos must be narrated to explain what is happening on screen.

### Checklist
- [ ] **Test Strategy Overview:** Recap of how testing was approached.
- [ ] **Test Table:** A structured table containing:
    - [ ] Test Number & Description.
    - [ ] Test Type (**Typical, Erroneous, Extreme**).
    - [ ] Data used.
    - [ ] Expected Result.
    - [ ] Actual Result (Pass/Fail).
    - [ ] **Video Timestamp:** (e.g., `Video 1 @ 02:15`) linking the specific test to the video evidence.
- [ ] **Trace Tables:** At least 2 trace tables covering complex algorithms (e.g., loops, logic).
- [ ] **Navigation Testing:** Proof that menus and links work.
- [ ] **SQL/Data Testing:** Proof that data loads, saves, updates, and deletes correctly.

### Marking Criteria (Top Band: 7-8 Marks)
* Clear evidence, in the form of carefully selected representative samples, that thorough testing has been carried out.
* Demonstrates robustness of the complete (or nearly complete) solution.
* Demonstrates that the requirements have been achieved.

---

## 5. Evaluation (4 Marks)

Reflecting on the success of the project.

### Checklist
- [ ] **Objectives Assessment:** Copy your initial objectives from the Analysis and state if they were Met/Partially Met/Not Met, with comments explaining why.
- [ ] **User Feedback:** Evidence of feedback from the end-user (e.g., an email or signed letter).
- [ ] **Feedback Analysis:** Discussion of the user's feedback (what they liked, what they didn't).
- [ ] **Maintenance/Improvements:** Discussion of how the system could be improved if you had more time (extensions). These must be SMART extensions.

### Marking Criteria (Top Band: 4 Marks)
* Full consideration given to how well the outcome meets all of its requirements.
* Detailed consideration of how the outcome could be improved.
* Independent feedback obtained of a useful and realistic nature, evaluated and discussed meaningfully.