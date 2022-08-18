import { createSlice } from "@reduxjs/toolkit"

interface SiteAppearanceState {
    darkMode: boolean
}

const initialState: SiteAppearanceState = {
    darkMode: false
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
        loadingSaveState: (state) => {
            var appearenceData = localStorage.getItem(loadStorageKey);
            var appearanceJson = appearenceData !== null ? JSON.parse(appearenceData) : null;
            if(typeof appearanceJson.darkMode === 'boolean') state.darkMode = appearanceJson.darkMode
        }
    }
})

export const {setDarkMode, loadingSaveState} = siteAppearanceSlice.actions;