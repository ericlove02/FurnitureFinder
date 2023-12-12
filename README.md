# ChAiR

https://github.com/ericlove02/FurnitureFinder/assets/53005525/07eaf976-b2db-49fe-91c8-6d637f5f9992

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.001.jpeg)

Eric Love, Preston Malaer, Trisha Narwekar, Erick Ordonez, Alex Tung, Bryce Watt


**Table of Contents**

1. [Introduction](#introduction)
   1. Problem Background
   1. Needs Statement
   1. Goals and Objectives
   1. ChAiR’s “Vibe” Defined
   1. Design Constraints and Feasibility
2. [Final Project Design](#final-project-design)
   1. System Overview
   1. Design Updates
      1. Criteria Matrix
      1. Feature Review
   1. Development Specifications
   1. Frontend Implementation
   1. AR Implementation
   1. Database Design
3. [MVP](#mvp)
   1. Validation and Testing
   1. Maintenance
   1. Societal Impact
4. [User Manuals](#user-manuals)
   1. Vibe Selection
   1. AR View
   1. All Furniture and Favorites
5. [Project Debrief](#project-debrief)
   1. Project Management
      1. Team Overview
      1. Updated Implementation Schedule
      1. Updated Division of Labor and Responsibilities
   1. Retrospective and Evaluation
      1. Did ChAiR Achieve Its Goals?
      1. Growth and Skills Learned
      1. Future of ChAiR
6. [References](#references)

## Introduction

1\.1 Problem Background

When moving into an unoccupied space, the new tenant may wish to purchase furniture like lamps, chairs, sofas, couches, and tables. However, many furniture options exist, rendering the task of furniture shopping an arduous one. Prominent furniture design company Ikea sells roughly 9,500 pieces of furniture alone[^1]; Ashley Furniture sells over 6,000 items in-store and online[^2], while a plethora of other furniture manufacturers offer their own products[^3]. With so many distinct furniture items available to purchase, the search for furniture that appeals to the furnisher’s[^4] taste is often aimless and time-consuming. Complicating matters, furniture prices have skyrocketed in recent years[^5], narrowing options for furnishers on a budget.

All these factors contribute to the hardship within the “furniture selection process,”in which a furnisher experiences a need for furniture, and searches online for an appropriate furnishing. With so many websites and so many options, the furnisher can spend a considerable amount of time examining furniture.

Even if the furnisher finds a furniture item that seems to meet their tastes, once the furniture is ordered and arrives, there exists the risk that the furnisher will be displeased with their purchase. From the furnisher’s point of view, the furniture may be a bad fit for the room, or the furniture may fit the space differently than the furnisher anticipated; this could have been avoided if the furnisher had a way to visualize the room with their purchase beforehand.

If the furnisher dislikes their purchase, they must either tolerate it or return the item and repeat the process. Overall, the frustration of furniture shopping frustrates the target audience.

2. Needs Statement

“There exists a need to see how furniture fits together before purchase from a first-person perspective. The vast number of furniture items and retailers results in spending time searching websites for thematically cohesive furniture, which can be haphazard and disjointed. There exists a need to simplify the process.”

3. Goals, Objectives, and Validation

If the furnisher had a tool that narrowed down their furniture options, while also providing a first-person visual of the furnisher’s space with that furniture, the problems described above can be remedied. Our application, ChAiR, aims to fulfill this role. To achieve this, we decided that the application at a minimum should:

- Possess an intuitive interface,
- Run on iOS and Android devices,
- Allow users to select a vibe,
- Provide furniture according to that vibe,
- Display that furniture in an AR interface of the users’desired room,
- Allow users to share their furniture selections on social media.

The incorporation of augmented reality in particular presents users with the means to evaluate the newly furnished room in a semi-simulated environment, allowing users to determine if their furniture choices are cohesive and correct for the room.

The vibe selector represents ChAiR’s hallmark feature. The “vibe”composes one part of the solution, allowing users to narrow their furniture options to an aesthetically cohesive selection, with furniture items corresponding to the user-selected vibe. This lines up with ChAiR’s central goal of streamlining and simplifying the furniture procurement process; the selection of a vibe will provide users with furniture that corresponds to that vibe, narrowing the user’s options.

To achieve these goals, the application must satisfy several criteria. ChAiR as a minimum viable product (MVP) must:

1. Function well on the iPhone 12 and Google Pixel 7a (measured in frames per second on these devices and storage usage),
1. Improve the experience of searching for furniture based on the user's intent where
1. Users who wish to select furniture as quickly as possible and are using ChAiR should spend less time searching for furniture compared to those who are not using the application,
1. Users who pursue a systematic approach to finding the appropriate furniture and are using the application should have fewer furniture permutations than those who are not using the app,
3. Enable users who are using the application to feel that the furniture they purchased fits together in the room compared to those not using the application.

The app’s performance centered on the concern for availability; if the app cannot run well on the devices the team wants to support, then the app will fail to improve the furniture selection process. Proper frames per second (FPS) measurements were conducted on the app through simulations of expected use, like filling out a living room with appropriate furniture.

The application’s vibe selection drop-down should ease furniture searching by providing a curated list based on the user’s choice of vibe. The application should ease the furniture selection process for multiple kinds of users; users who wish to find furniture for their room quickly, and users who wish to spend more time searching for cohesive furniture should find their needs met with ChAiR. For users looking to reduce the time spent searching for furniture, the curated furniture options will limit the time those users spend combing through websites, as options will be presented to them. Meanwhile, users wishing to pursue a more thorough approach can consider fewer furniture options while still receiving a collection of furniture that matches their tastes. “Furniture permutations”in this case refers to the number of different furniture items that the shopper considered before settling on their final choice.

The furniture curation, detailed in later sections, depends on the vibes assigned to each piece of furniture.

Users of ChAiR should leave the experience feeling confident that the furniture they purchased would fit well in their space. The inclusion of AR (which will allow users to see the furniture in the room) should enable users to feel more confident about their furniture choices. In particular, the combination of AR and the vibe selection tool will allow users to know that both their furniture options match a specific vibe, while also being able to visualize those furniture options existing in the AR space.

User certainty that the furniture fits the space was measured through several tests, in which tested users will simulate a purchase of a furniture item without the app (no actual furniture was purchased).

4. ChAiR’s “Vibe” Defined


![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.004.jpeg) ![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.005.jpeg) ![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.006.jpeg)

Cottagecore, Minimalist, and Modern Vibes

A“vibe”can be a nebulous and subjective concept, but primarily revolves around the feeling generated by the observer. In the above examples, the positioning and selection of furniture contribute to the feeling invoked in the viewer. For instance, “minimalist”appears plain and should conjure an atmosphere of simplicity. The white furniture and limited decorations contribute to that feeling. Meanwhile, cottagecore should feel homely and lived-in.

Each furniture item will have its vibes. An individual furniture item’s vibes were determined before inclusion by a “vibe committee”, three individuals with design experience[^6] tasked with assigning each of ChAiR’s furniture items appropriate vibes. The vibe committee will examine scenes and pictures like examples displayed, and determine based on comparison how well each piece of furniture corresponds to each vibe. The furniture will be assigned the vibes it most heavily resembles.

5. Constraints, Challenges, and Feasibility

The team had to consider the limitations of both the iPhone 12 and the Google Pixel 7a; if those devices lacked the space or were unable to run the application with an acceptable FPS during regular use, then ChAiR needed to change to accommodate them. During testing, the team used 30 FPS as a benchmark for an “acceptable”framerate. The team has been using 30 FPS in particular because it represents a marriage between balanced performance and application stability; aiming for a higher FPS risks straining the hardware, while a lower FPS risks the user experience. In this case, “regular use”of the app assumes the placement and display of a reasonable number of furniture items, akin to what one may observe in a crowded living room.

Likewise, if the application used an unacceptable amount of storage, such as 10 gigabytes or more, then the attractiveness of the application diminishes. As the app stores furniture models locally, each of which takes up roughly 120kb, only so many furniture items can be supported before the size of the application borders on absurdity. The team has settled on an application size of 300 megabytes as an upper limit; this will allow the application to support over a thousand furniture items while remaining at a reasonable size. The Capstone version of the project stores only enough models to demonstrate the application’s capabilities, but 300 megabytes existed as a limit for the team to guide development. This number was chosen because it allowed for a great deal of flexibility while still guaranteeing to hold the features the team wishes to package with ChAiR.

The available time presented a significant challenge. The development team possessed only a few weeks to deliver the minimum viable product; however, every member of the development team was a student with other commitments, limiting the actual time that could be allocated to engineering.

The team only had a budget of $500; as such, the team has avoided any APIs that charge based on usage.

## Final Project Design

2\.1 System Overview

Our system used Unity for the majority of the application handling, with a ![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.007.png)MySQL database and a PHP backend for creating routes to the data. Within Unity itself, the project’s front end was built out from objects in scenes in the engine, which were then manipulated with C# scripts compiled in Unity and run within the scenes. We created a MySQL database to store any data our application needs. We created a variety of routes in PHP and hosted them on 000webhost to allow the C# scripts in Unity to easily access the routes and thus our database to use the data within the application.

Our database was manually filled with furniture information from Polantis, a furniture model resource; the models will be used to create prefabs in Unity which is stored locally on the device. The index of this prefab is stored in the database to be used by the front end when a furniture item needs to be loaded. After a user has created their room, they can select objects they like and view the product information, including manufacturer links. Opening these links brings the user to an external browser to view the manufacturer's purchase page.

2. Design Updates

**2.2.1 Criteria Matrix**

After careful evaluation of the user experience and process from end to start along with considering the separation of duties among team members- our development team has identified various features not found in collaboration with existing competitors on the market. The features identified are listed as IOS & Android compatibility, Vibe Selection, Social Media Shareability, Purchasable Furniture Options, and AR Layout Visualization. While being used both separately and together in existing services, it was found through market research and competitor analysis that most tools targeted specific individual markets rather than looking to encompass a broader sector. Such as Pinterest's focus on brainstorming certain vibes rather than making it a reality through purchasable options/specific layout specifications. Or, on the contrary VRApartments’detailed VR layout rough drafts but lack of vibe selection leading to an effective furniture placement tool but no way to create cohesion between objects being placed. Displayed below is a table showing the criteria matrix detailing the desired feature set combination that makes ChAiR stand out from the competition in the current market.



|**Features**|**VRApartments**|**Apple Measure**|**Pinterest**|**Amazon AR View**|**ChAiR**|
| - | - | - | - | :- | - |
|IOS & Android Compatibility|✓||✓|✓|✓|
|Vibe Selection|||✓||✓|
|Social Media Shareability|✓|✓|✓||✓|
|Purchasable Furniture Options||||✓|✓|



|AR Layout Visualization|✓|✓|||✓|
| :- | - | - | :- | :- | - |



|**Competitor 1 VRApartments**||
| - | :- |
|IOS &Android Compatibility|<p>✓ VRApartments did user-friendly IOS and Android compatibility by being</p><p>readily available on both Google and Apple App Store with features on both platforms being the same.</p>|
|Vibe Selection|<p>✗ They did not however offer any type of vibe selection aspect with their</p><p>purpose seeming to be more of a layout design prototyping tool rather than synergetic decoration.</p>|
|Social Media Shareability|<p>✓ This tool did in fact have options to save and share one’s desired layout to</p><p>social media for public opinion on their design choices.</p>|
|Purchasable Furniture Options|<p>✗ As stated before the purpose of this tool didn’t seem to be realistically</p><p>populating the apartment with cohesive decorations but rather to plan the setup of the space itself- therefore lacking the ability to purchase real-world items.</p>|
|AR Layout Visualization|<p>✓ This platform did contain an AR/VR layout view visualization, probably the</p><p>best in fact out of all the competitors with precise measurements and dimensions being able to be shown in virtual reality.</p>|



|**Competitor 2 Apple Measure**||
| - | :- |
|IOS &Android Compatibility|<p>✗ This app did not have Android compatibility because it is an</p><p>Apple-made app and not offered elsewhere but in the App Store.</p>|
|Vibe Selection|✗ There was no vibe selection feature offered for this app.|
|Social Media Shareability|<p>✓ Apple Measure did have a feature to screen capture and then have a quick</p><p>link to share to one’s favorite social media platform.</p>|
|Purchasable Furniture Options|<p>✗ The ability to purchase any type of real-world furniture was missing from</p><p>this app.</p>|
|AR Layout Visualization|<p>✓ This tool had a great AR layout visualizer where users could easily see</p><p>all their measured items and pin them- however, as far as furniture placement and object location those aspects were lacking.</p>|



|**Competitor 3 Pinterest**||
| - | :- |
|IOS &Android Compatibility|<p>✓ Pinterest probably had the most user-friendly cross-platform</p><p>compatibility with them being readily accessible via app on both Google Play and App Store as well as there being a website access option.</p>|
|Vibe Selection|✓ This is most definitely the best option among the competitors as far as|



||vibe selection and looking for a vibe for research purposes- there are plenty of users on this app and that leads to plentiful ideas on various cohesive furniture themes.|
| :- | :- |
|Social Media Shareability|<p>✓ Pinterest had readily available features to export and share to social</p><p>media as it is a social media platform in a way and there were also quick links to share to various other apps.</p>|
|Purchasable Furniture Options|<p>✗ Perhaps the biggest flaw of this app is the fact that it can produce such</p><p>great vibe ideas but they tend to most of the time stay theoretical with it</p><p>being a pain to find the real-world objects to purchase to make your desired vibe a reality.</p>|
|AR Layout Visualization|<p>✗ Also, another feature lacking in this tool, was there was no way to view</p><p>various themes in AR view so one would not have a clue if they fit in their own living space.</p>|



|**Competitor 4 Amazon AR View**||
| - | :- |
|IOS &Android Compatibility|<p>✓ This platform did offer IOS and Android compatibility similar to</p><p>Pinterest with them also having a website view option as well.</p>|
|Vibe Selection|✗ There was no vibe selection feature for this platform.|
|Social Media Shareability|<p>✗ This tool did not have an option to export the current image and neither</p><p>an option to export it to social media platforms via quick links.</p>|



|Purchasable Furniture Options|<p>✓ Amazon is the king of providing purchasable options with them having</p><p>perhaps the most extensive selection of furniture options on the market.</p>|
| - | :- |
|AR Layout Visualization|<p>✗ While this app did offer an AR view it was only for one item and not being</p><p>able to view an entire layout or multiple objects at once so you wouldn’t be able to see an entire desired vibe in your living space.</p>|

**2.2.2 Feature Review**

User Features

- IOS &Android Compatibility: User access is offered on both IOS and Android platforms.
- Vibe Selection: The ability to select a vibe from a dropdown menu and there

  being an example image displayed before entering AR view. Once in AR view furniture options are then narrowed down to the vibe selected.

- Social Media Shareability: The option to screen capture the current layout of one’s camera roll as well as the ability to share to one’s favorite social media platforms via quick links.
- Purchasable Furniture Options: The ability to purchase real-world furniture via links to the official distributor of various furniture options offered in the app.
- AR Layout Visualization: User ability to see their cohesive furniture and decoration layout in augmented reality view, providing an accurate representation of their desired vibe selected.
3. Development Specifications

**2.3.1 Frontend Implementation**

We chose to complete the UI/UX design in Figma, an online design tool for web and mobile applications. Our front end will be developed in Unity. Unity frontend allows us to integrate the AR into our app seamlessly. It can support ARCore and ARKit, thus making it highly accessible across platforms. Unity provides

many tools for creating the UI we want. It also has many customizations for creating the experience we aim for our users to have. Because Unity is an engine made for game development, it also optimizes the resources used by the user interface. This means that it can keep the app running smoothly without overutilizing system resources.

Our frontend begins on a home screen which features a dropdown menu with multiple vibes to select from. When a vibe is selected, the AR view button will appear and the user may click it. Once clicked, the AR view has a selection bar of furniture which allows the user to drag and drop a basic piece of furniture. Then, they can flip through multiple types of furniture, all of which are within the

selected vibe. From the home screen, a user can also access a side navigation bar where they can see all products or scroll through their favorites.

2. **AR Implementation**

For our AR implementation, we created an AR environment that can position and rotate objects. The AR was also developed in Unity. Unity has many tools that can be used to adapt AR capabilities to multiple mobile platforms and also manage AR support that may be more difficult without an engine.

Furniture placement and rotation have been implemented in the AR space. Within the AR space, furniture items can be rotated 360 degrees. It can display all furniture items based on vibe by flipping through the multiple options. The AR environment also detects collisions between furniture items. The furniture will be scaled to the size of the real furniture model. It will be accessible to most mobile platforms.

3. **Database Design**

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.010.png)

As can be seen in the diagram above, there are three main components to our total backend design: a Furniture table, a FurnitureVibes table, and a Vibes table. Our Furniture table holds all of the information about each piece of furniture that you can place in the augmented reality space. Some things each row holds are a primary key ID, the furniture type, dimensions in 3D coordinates (height, width, length), and the cost, amongst other things. The furniture table was designed in this manner to store the information about each piece of furniture and connect it to the FurnitureVibes table. The FurnitureVibes table simply contains the furniture ID as well as the vibe ID. Its purpose is to build a bridge between the Vibes table and the Furniture table. Lastly, the Vibes table serves as a way to store the Vibes that our team has defined. We have the ID of the vibe as well as the name, which will be used to decide which furniture is shown in the AR section.

The current setup in MyPHPAdmin is depicted below.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.012.png)

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.013.png)

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.014.png)

## MVP

3\.1 Validation and Testing

To validate that we have accomplished what we set out to accomplish with ChAiR there are several distinct aspects of the project that need to be tested.

- Function well on the iPhone 12 and Google Pixel 7a (measured in frames per second on these devices and storage usage).
  - The goal is to have the app be as light as possible. Asize less than or equal to 300 MB means we have accomplished this goal.
  - Frames per second will be tested in a variety of different scenarios.
    - Both phones should perform at 30 Frames per second or faster in all scenarios
    - These scenarios will include a large room and a small room both with many pieces of furniture and few pieces. We will also be testing performance when the furniture is rapidly regenerated
- Reduce the time users spend searching for furniture compared to those not using the app.
  - Agroup of users will be asked to find a certain number of furniture pieces that fit well together. We will measure how long on average it takes them to do that. Then we will ask them to do the same thing but using our app. In order to ensure that the order does not have an undue effect we will have a second group of people first use our app and then decorate a room without our app.
  - We will consider this a success if both groups see a significant decrease in time spent when using our app.
- Allow users to feel more confident about the furniture they have purchased (evaluated through confidence surveys, with confidence ranked on a scale of 1-10).
  - This will include an open-ended question asking the users to describe their experience as well as a few qualitative questions
  - These questions will be similar to the following:
    - Have you ever shopped for furniture before?
    - On a scale of 1-10, in those previous experiences, how confident were you in your furniture choices before they arrived at your home?
    - On a scale of 1-10, how confident would you be in purchasing the furniture that you found using this app?
    - On a scale of 1-10, with 1being strongly disagree and 10 being strongly agree, how much do you agree with this statement: “Using ChAiR, it was much easier to find furniture”?
    - On a scale of 1-10, with 1being strongly disagree and 10 being strongly agree, how much do you agree with this statement: “I am much more confident in my purchases using ChAiR than I would be without it”?
- Furniture placement in AR needs to be as realistic as possible
  - Furniture needs to be sized correctly
    - We will take reference measurements of the room in which we place the furniture and compare it to the provided dimensions of the furniture to ensure that the sizing is consistent.
  - Furniture needs to look like the furniture the customer can purchase
    - We will compare each model and texture to its reference image to ensure consistency
    - We will check each purchase link provided to the user to ensure that they go to the correct product
  - Furniture should not be able to be placed in impossible ways
    - Furniture should not clip into other furniture
      - We will attempt to place clip furniture into each other in all of the ways that we can think of to ensure that it is impossible
      - Includes but is not limited to initial placement of furniture, moving furniture, and size discrepancies due to regeneration of furniture.
    - Furniture should only be allowed to be placed in the bounds of the room
      - We will attempt to place furniture outside of the room in the same way that we will test for furniture clipping.
- Furniture needs to match the chosen vibe
- As a team, we will review each vibe and cycle through the recommendations to ensure that they all look cohesive and that each vibe is distinct from the others.
- Ensure that the images captured when exporting a scene are saved to the device or properly shared to whichever media that the user chooses.
- We will be testing each aspect of the export process by hand.
2. Maintenance

Due to the fact that this was not a sponsored project, there will likely not be much actual maintenance after the duration of this semester. However, if we were going to maintain this project after the fact, the largest area of maintenance would be in updating our curated library of furniture. This would include adding new furniture, removing furniture that is no longer being sold, and adding new vibes.

One possible future feature would be opening up vibe curation to people not associated with the development of the app, like influencers. This could involve allowing influencers to sift through our library of furniture and create their curated vibes for their followers to use instead of just the ones that

3. Societal Impact

The process of choosing furniture can be very challenging and frustrating, especially if you cannot see the furniture side by side to make sure it will all look good together. ChAiR’s AR view aims to eliminate much of the frustration and indecision surrounding procuring furniture, by allowing the user to see firsthand how their furniture will look before they buy anything.

Returning furniture can also be difficult, especially large furniture. ChAiR will allow the user to experiment with different styles before purchasing, raising the likelihood that they are satisfied with the first piece of furniture that they buy. This in turn reduces the likelihood of the user needing to go through the process of returning their bulky furniture items.

ChAiR will also allow the user to try out different furniture layouts. This allows people who may not be physically able to rearrange the room to independently visualize a final layout before they begin the physically demanding process of moving furniture physically into the space. This will minimize the physical work necessary to achieve a layout that they are satisfied with and, therefore, make the process of decorating a room much more accessible to those with physical disabilities.

## User Manuals

4\.1 Vibe Selection

When users open the application they will be greeted by a dropdown menu listing out several “vibes.”Adescription of what exactly a vibe means in the context of this application has been detailed earlier in this document, but in short, it is the look and feel of the room and furniture they are seeking.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.016.jpeg)

Once the user has selected their desired vibe they will be shown a scrolling image view of examples of what the vibe looks like in other rooms. If this isn’t what they’re looking for, they can use the dropdown menu to select a new vibe and the image examples will be updated to show the new vibe. Once the desired vibe is selected, they can press the AR View button to enter the AR view.

 ![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.018.jpeg)![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.019.jpeg)

2. AR View

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.020.jpeg)

Once the user enters the AR view, they can pan their phone around the room to detect AR planes in their space. This will place gray objects over the floor that they can use to create their space. The view has a selection bar at the bottom with icons depicting a sofa, chair, lamp, table, desk, and drawers. Each of these icons can be dragged from the bar into an AR plane and will be replaced with a 3D model of a piece of furniture matching the vibe that the user previously selected.


Once the piece of furniture exists as a model in the space, the user can click on it to select it, giving them several buttons to choose from to take action on the object. The user can click and hold on the four directional arrow icon, then drag it to another position on any AR plane to change its position. If they drop it off a plane it will return to its position. The user can click and slide the axis rotate icon left and right to rotate the object on the y-axis. If the user doesn’t like the furniture item that was randomly selected, they can press the rotational icon to regenerate the furniture item with a new piece of the same vibe and type. The user can press the X icon to remove the model from the view.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.022.png)

To view more information about the item, the user can press the information icon. This will open a panel showing the name of the item, the cost, its dimensions, a description, and a button that will open an external link to purchase the item. If the user likes the item and wants to save it to their favorites they can press the heart icon. If it is already in their favorites, the heart will be shown as a filled heart and can be pressed to remove it.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.023.png)

If the user places models colliding with one another, both models will be given a red aura, showing that they will not be able to be placed in this configuration in the real world. If the user makes adjustments to the position, or rotation, or deletes an object, these auras will be updated.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.024.png)

As the user places items into the view, a total of the cost of the furniture items will be kept in the top right corner of the screen. If the user presses on the drop down of this total they can see an itemized list of the items making up this number.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.027.jpeg)

When the user initially enters the AR View, if models are missing from their local storage that are available on the application's database, the user will get a message prompting them to update the app to get the new models.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.026.jpeg)

The user can press the export button to take a capture of their room configuration. They can save this image to their device or share it using their device's share options.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.029.jpeg)

3. All Furniture and Favorites

The All Furniture page displays a list of every furniture item supported by the application. Users can reach it by pressing the menu button in the upper-left corner of the home screen, followed by tapping the “All Furniture”button.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.031.png) ![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.032.png)

*The Home Page Before and After Tapping the Menu Icon!*

After tapping the “All Furniture”button, users are presented with the “All Furniture”page, the screen displaying the list of every furniture item supported by the application. On this screen, users will be able to scroll up and down to see every furniture item. When a specific furniture button is tapped, the app will generate a panel containing details about the selection, such as object dimensions, a brief description, and a link for purchasing the furniture. On this page, users can also favorite furniture items.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.033.png) ![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.034.png) ![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.035.png)

*The All Furniture Page, with a Selected Furniture Panel with and without a Favorite!*

When a user favorites furniture either within the AR View or the All Furniture page, that furniture can be seen on the favorites page. Accessible through the same side menu as the all furniture page, the favorites page will display only the furniture favorited by the user. Otherwise, the favorites page mechanically behaves in the same way as the All Furniture page, allowing users to select furniture items and generate an informative panel.

![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.036.png) ![](/ChAiR-Media/Aspose.Words.882d5acd-be1c-4576-be83-6a04ee5a984a.037.png)

*The Favorites Page and a Viewed Favorited Item!*

Furniture options disappear from the Favorites page when they are unfavorited. Auser’s favorites are saved locally on the device itself, allowing users to keep track of furniture items that were especially to their tastes between sessions.

The All Furniture and Favorites pages exist as a way to organize the furniture in the application. When used in concert with the AR view, users can both find furniture options that match their tastes while tracking those items for later purchase.

## Project Debrief

5\.1 Project Management

The team leader ensured tasks were completed, and led the team meetings. Each group member took tasks for the week and continued to work on their tasks until the next in-person meeting. In order to develop the overall project, the team came! together to make decisions for the application.

Each week the team met on Tuesday and Thursday. One was a thorough meeting where tasks were decided as well as future goals. The other was a meeting to check in and ensure that all team members had an understanding of the next steps. There was also daily communication over Discord with updates. Versioning was done using Github.

**5.1.1 Team Overview**



<table>
  <tr>
    <td>
      <p><strong>Preston Malaer</strong></p>
      <p><em>Background:</em> Cybersecurity, Data Analytics</p>
      <p><em>Role:</em> Team Leader & Full Stack</p>
      <p><em>Responsibilities:</em></p>
      <ul>
        <li>Replacing Old in AR</li>
        <li>Displaying New Furniture in AR</li>
        <li>Validating Furniture size</li>
        <li>Vibe Determination</li>
      </ul>
    </td>
    <td>
      <p><strong>Eric Love</strong></p>
      <p><em>Background:</em> Web Development, Database</p>
      <p><em>Role:</em> Full Stack</p>
      <p><em>Responsibilities:</em></p>
      <ul>
        <li>App Construction</li>
        <li>Displaying Room in AR</li>
        <li>Validating Furniture Size</li>
      </ul>
    </td>
    <td>
      <p><strong>Trisha Narwekar</strong></p>
      <p><em>Background:</em> Web development, cloud integration, database systems</p>
      <ul>
        <li>Database Setup</li>
        <li>Furniture Uploading</li>
        <li>Linking Database & App</li>
        <li>Server Infrastructure & App</li>
      </ul>
    </td>
  </tr>
  <tr>
    <td>
      <p><strong>Erick Ordonez</strong></p>
      <p><em>Background:</em> Data Science / Analytics, Database, Statistics</p>
      <p><em>Role:</em> Backend</p>
      <p><em>Responsibilities:</em></p>
      <ul>
        <li>Furniture Searching</li>
        <li>Displaying New Furniture in AR</li>
      </ul>
    </td>
    <td>
      <p><strong>Alex Tung</strong></p>
      <p><em>Background:</em> Web Development, scripting, database</p>
      <p><em>Role:</em> Full Stack & UI/UX designer</p>
      <p><em>Responsibilities:</em></p>
      <ul>
        <li>UI Mockups</li>
        <li>Homepage</li>
        <li>Database Setup</li>
        <li>Linking Database & App</li>
      </ul>
    </td>
    <td>
      <p><strong>Bryce Watt</strong></p>
      <p><em>Background:</em> Web development, game development</p>
      <p><em>Role:</em> Full Stack</p>
      <p><em>Responsibilities:</em></p>
      <ul>
        <li>Vibe Determination</li>
        <li>Replacing Old Furniture in AR</li>
        <li>AR Interface</li>
      </ul>
    </td>
  </tr>
</table>


2. **Updated Implementation of Schedule**

Each week we checked the progress of the Gantt Chart in order to make sure we stayed on schedule with the project.


This is our final updated Gantt Chart. We have finished all tasks except for “Deployment on App Store”.

3. **Updated Division of Labor and Responsibilities**

Since the CDR, our main responsibilities stayed the same, and we were able to finish all of the tasks on the Gantt chart except for one: Deployment on App Store.

Since the CDR, Eric Love implemented a Dimensions Feature and Collisions Feature. Prestone constructed the All Furniture and Favorites Pages. Bryce implemented a sharing feature for both Android and IOS. Trisha Implemented a Cost Feature to track the running cost from furniture items chosen by the user. Alex populated the Database with all the models we sourced. Erick found the rest of the furniture models needed. Alex, Trisha, and Erick assigned vibes to all models using the process that Preston designed. In the final days before the demo, all of us worked on creating Prefabs and Sprites for the furniture models.

Apart from all of us working on Prefabs and Sprites on the days leading up to the demo, our main roles and responsibilities stayed the same.

5\.2 Retrospective and Evaluation

**5.2.1 Did ChAiR “Succeed?”**

As the development process for ChAiR solidified, the team set forward a plethora of goals and constraints to bind the development of the project.



|ChAiR Goals/Constraints vs. Success||
| - | - |
|App size less than 300 megabytes.|✓|
|<p>App runs at 24 FPS (frames per second)</p><p>on the AR portions.</p>|✓|
|<p>Support at least two furniture items</p><p>per vibe per type.</p>|✘|
|<p>All available furniture options are</p><p>purchasable.</p>|✓|
|“Thorough”users using the app use less “furniture permutations”than when not using the app.|✓|
|“Time-sensitive”users using the app find furniture faster than when not using the app.|✓|

The “harder”constraints (app size and framerate) both met our criteria. On the Google Pixel 7a and the iPhone 12, the application consistently ran at 30 frames per second or above under regular usage. Though filling a room with unreasonable amounts of furniture and forcing many collisions did cause the application to slow, “expected usage”(populating the space with 10-15 furniture items to evaluate the aesthetic) ran above our 24 FPS threshold. Meanwhile, the application itself occupies 194 megabytes, well below the 300 megabyte threshold with room for many more furniture items.

The final version of the application supported 6 types of furniture (sofas, chairs, maps, tables, desks, and drawers) and 9 separate vibes. To meet the appropriate number of furniture items per vibe, we would need at least two of each furniture for each vibe. Though the vast majority of the furniture items in most of the vibes met this or surpassed it, the vintage and art deco vibes lacked enough drawers and tables, respectively. Combined with the difficulty in both acquiring and refining the models, some furniture models were not acquired in time. Though the application supported 177 furniture models, when evaluated as a binary, it cannot be said this criteria was satisfied.

All of the furniture options that the application does support have a purchasable link, satisfying the third constraint.

When evaluated, test subjects[^7] went through fewer “furniture permutations” on average when using the app compared to those not using the app. As such, this constraint is satisfied; however, this could be affected by more furniture options. As more furniture is added to the app, the average number of furniture permutations should be re-evaluated. Further, this constraint only applies to thorough users wishing to find the “perfect”furniture options for them. The test subject was prompted with, “be thorough in finding a table, a chair, and a sofa that you feel is best for this space.”With this prompt, test users[^8] on average considered 15 tables, 25 lamps, and 40 drawers before settling on their final option; meanwhile, test users using ChAiR considered only 3 furniture options. One of the test users suggested that seeing the furniture in AR helped them feel more confident about their choice. During one furniture permutation test where the user was asked to be thorough without the application, the subject immediately opened up Pinterest, indicating an overlap between Pinterest’s usage and ChAiR’s.

For users simply wishing to find a furniture item quickly and with little hassle, test subjects using the application found furniture items faster than users not using the application. On average, test users not using the application spent 2 minutes and 36 seconds searching for furniture, while test users using ChAiR spent an average of 2 minutes and 7 seconds. Test subjects also verbally noted feeling more confident about their choices when they were able to see them depicted in the space. Like with the “furniture permutations”criteria, this could be affected by the amount of furniture supported by the application. Further, while 19 seconds is a considerable improvement and enough to satisfy the constraint, future development should focus on further reducing the time this type of ChAiR user spends searching for furniture items.

2. **Growth and Skills Learned**

When the team was initially deciding on a Capstone project, we were           debating a safer, less ambitious option or a riskier, more demanding option: ChAiR. Ultimately, we decided to challenge ourselves, starting on the trail to ChAiR with    the path ahead lit only by our initial idea: an AR application which allows users to see furniture. We also knew we wanted to incorporate the idea of “vibes,”although the concept remained hazy at the start of the semester. None of the team had any significant experience with AR, 3D modeling, or even interior design. Despite this, the idea’s potential motivated the team to understand these concepts more, resulting in extensive research and a detailed understanding of our options.

We chose this project because we knew it would test our abilities, giving us opportunities to learn and grow as developers. This goal even bled into development; even if one team member had more experience with C# scripting, we would still assign a C# task to a less experienced team member to allow them the opportunity to grow. In this way, growth existed as an invisible project criteria. When the final version was presented, each member of the team had worked with an alien concept to them, forcing growth through application. In particular, no team members had worked with AR or Unity before; by the end of the project, each member of the team had worked extensively with Unity and contributed to the AR component of the application.

3. **The Future of ChAiR**

With the MVP for ChAiR now complete, the current development team anticipates moving on from the project. Instead, the application will be open-sourced, with the GitHub page made public for forking and public contributions. ChAiR depends on furniture models for its usefulness, so the application will continue to benefit from additional models. The quality of the furniture items themselves could be improved, while some of the unachieved stretch goals could see development.

## References

**Competitors**

[https://vrpartments.com/ ](https://vrpartments.com/)

[https://www.amazon.com/products ](https://www.amazon.com/products)

[https://www.pinterest.com/ ](https://www.pinterest.com/)

<https://support.apple.com/en-us/HT208924>

**Furniture Sources**

<https://www.polantis.com/>

**Problem Background**

[https://www.forbes.com/companies/ikea/?sh=236b271c2ad0 ](https://www.forbes.com/companies/ikea/?sh=236b271c2ad0)

[https://www.countryliving.com/shopping/a43248/ashley-furniture/ ](https://www.countryliving.com/shopping/a43248/ashley-furniture/)

[https://www.microdinc.com/furniture-manufacturers-list/ ](https://www.microdinc.com/furniture-manufacturers-list/)

[https://www.housedigest.com/1079297/the-price-of-household-furniture-has-gone-up-dr astically/](https://www.housedigest.com/1079297/the-price-of-household-furniture-has-gone-up-drastically/)

[^1]: <https://www.forbes.com/companies/ikea/?sh=236b271c2ad0>

[^2]: <https://www.countryliving.com/shopping/a43248/ashley-furniture/>

[^3]: <https://www.microdinc.com/furniture-manufacturers-list/>

[^4]: For the purposes of this document, someone who populates their home with furniture. We are calling them furnishers as an indication that they are not yet users since they have not utilized our app yet.

[^5]: <https://www.housedigest.com/1079297/the-price-of-household-furniture-has-gone-up-drastically/>

[^6]: For the purposes of the Capstone version, the committee will be composed of the development team.

[^7]: Test subjects were members of the developers team and those in their immediate social circle.

[^8]: The terms “test users”and “test subjects”refer to the same group, and exist as synonyms here.
