import { Box, TextField } from '@material-ui/core'
import React from 'react'

const TextBox = ({text, required, type}) => {
  return (
      <TextField margin="normal" fullWidth id="outlined-basic" label={text} variant="outlined" required = {required} type = {type}/>
  )
}

export default TextBox