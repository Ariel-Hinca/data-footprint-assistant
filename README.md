# Data Footprint Assistant

A private app that runs only on your computer. It helps you keep track of data brokers, write a removal or opt-out letter, and see what is still pending.

Your name, email addresses, phone numbers, notes, and activity stay in a database on this PC:

`%LOCALAPPDATA%\DataFootprintAssistant\footprint.db`

The app does not send them to a server. Search buttons only open a Google tab. Opt-out links only open the broker's own page. Nothing is scraped or filled in for you.

The letters are drafts for you to read. They are not legal advice. If a broker has its own form, use that form.

## Run it

1. Open `DataFootprintAssistant.sln` in Visual Studio.
2. Press F5.

Or, from this folder:

```powershell
dotnet run --project src/DataFootprintAssistant
```

Then open http://localhost:5017

## What is included

The broker list starts with a few well-known sites and the official opt-out page for each one. You can add other brokers yourself. Changing the starter list does not erase your statuses or notes.
