import {useState} from "react";
import {Box, Divider, Slider, Typography} from "@mui/material";

interface Props {
    minValue: number;
    maxValue: number;
    selectedValue?: { min?: number | null, max?: number | null }
    onChange: (min: number, max: number) => void;
    label: string;
}

export default function MinMaxSliders({minValue, maxValue, selectedValue, onChange, label}: Props) {
    const [value, setValue] = useState([
        selectedValue?.min || minValue,
        selectedValue?.max || maxValue])

    const marks = [{
        value: minValue,
        label: minValue.toString()
    },
        {
            value: maxValue,
            label: maxValue.toString()
        }]
    
    const handleChange = (event: Event, newValue: number | number[]) => {
        let tempValue = newValue as number[];
        setValue(tempValue);
        onChange(tempValue[0], tempValue[1]);
    }

    return (
        <>
            <Typography variant='h6'>{label}</Typography>
            <Divider />
            <Box sx={{px: 2}}>
                <Slider value={value}
                        onChange={handleChange}
                        valueLabelDisplay='on'
                        marks={marks}
                        step={1}
                        min={minValue}
                        max={maxValue}
                        sx={{mt: 5}}
                />
            </Box>
        </>
    )
}