import { Box, Grid } from '@material-ui/core'
import React from 'react'
import LoginForm from './LoginForm'
import Logo from './Logo'


const LoginPage = () => {
  return (
    <Box position="absolute" sx={{top:0, bottom:0, left:0, right:0, alignItems:'center'}} >
      <Grid container spacing={1} style={{height:'100%'}} alignItems='center'>
        <Grid item xs={0} sm={7}>
            <Logo/>
        </Grid>
        
        <Grid item xs={12} sm={5} style={{alignContent:'center',height:'60%'}}>
          <LoginForm/>
        </Grid>
      </Grid>
    </Box>
  )
}

export default LoginPage