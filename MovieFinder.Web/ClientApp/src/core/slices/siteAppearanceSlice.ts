import { createSlice } from "@reduxjs/toolkit"
import {ListView} from "../enums/listView";

interface SiteAppearanceState {
    darkMode: boolean,
    listView: ListView
}

const initialState: SiteAppearanceState = {
    darkMode: false,
    listView: ListView.Grid
}

const loadStorageKey = "siteAppearence";

export const siteAppearanceSlice = createSlice({
    name: 'siteAppearance',
    initialState,
    reducers: {
        setDarkMode: (state, action) => {
            state.darkMode =  action.payload;
            localStorage.setItem(loadStorageKey, JSON.stringify(state));
        },
        setListView:(state, action) => {
            state.listView = action.payload;
            localStorage.setItem(loadStorageKey, JSON.stringify(state));
        },
        loadingSaveState: (state) => {
            var appearenceData = localStorage.getItem(loadStorageKey);
            var appearanceJson = appearenceData !== null ? JSON.parse(appearenceData) : null;
            if(typeof appearanceJson?.darkMode === 'boolean') state.darkMode = appearanceJson.darkMode;
            if(typeof appearanceJson?.listView === 'number') state.listView = appearanceJson.listView;
        }
    }
})

export const {setDarkMode, loadingSaveState, setListView} = siteAppearanceSlice.actions;