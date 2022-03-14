import { Box, Divider, Grid, Link } from '@material-ui/core'
import React from 'react'
import TextBox from './TextBox'
import Btn from './Btn'

const LoginForm = () => {
  return (
        <Box sx={{bgcolor:'text.disabled', display:"flex", width:"70%",height:'100%',p:'20px', borderRadius:5, border:'5px'}}>
            <Grid container spacing={2} style={{height:'100%'}} alignContent='center'>
                <Grid container direction="column"  item xs={12} spacing={2} style={{width:'100%'}} alignItems='center'>
                    <Grid item style={{width:'75%'}}>
                        <TextBox text="Email" required="true" type="text"/>
                        <TextBox text="Password" required="true" type="password"/>
                    </Grid>
                </Grid>
                <Grid container item  xs={12} direction="column"  spacing={2} alignItems='center'>
                    <Grid item style={{width:'75%'}}>
                        <Btn color="primary" text="Login" ></Btn>
                    </Grid>
                    <Link href="#" underline="always">Reset Password?</Link>
                    <hr style={{width:'80%', backgroundColor:'black', height:'1px', border:0}}/>
                    <Grid item>
                        <Btn color="" text="Register" size='large'></Btn>
                    </Grid>
                </Grid>
            </Grid>
        </Box>
     )
}

export default LoginForm