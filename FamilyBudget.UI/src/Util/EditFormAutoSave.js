import _ from 'lodash'
import {useEffect, useRef} from 'react'
import {useEditContext} from 'react-admin'
import {useForm, useFormState} from 'react-final-form'

const EditFormAutoSave = ({waitInterval = 1000}) => {
    const {dirty, valid, values} = useFormState({
        subscription: {
            dirty: true, valid: true, values: true,
        }
    })
    const {submit} = useForm()
    const {saving} = useEditContext()
    const shouldSubmitRef = useRef()
    const submitDebouncedRef = useRef()
    /*
     * Determine whether 'submit' should be called by any of the following effects. Use a
     * 'ref' instead of a 'state' so that the an unmount effect can be set up which musn't
     * have state dependencies.
     */
    useEffect(() => {
        shouldSubmitRef.current = dirty && valid && !saving
    }, [dirty, saving, valid])
    /*
     * Debounce the 'submit()' function and store it in a 'ref' for the same reason as
     * above (it needs to be called on unmount which musn't have state dependencies).
     */
    useEffect(() => {
        submitDebouncedRef.current = _.debounce(submit, waitInterval)
    }, [submit, waitInterval])
    /*
     * Whenever the form data got dirty, schedule saving data
     */
    useEffect(() => {
        if (shouldSubmitRef.current) {
            submitDebouncedRef.current({...values}, /* redirectTo= */false)
        }
    }, [dirty, saving, valid, values])
    /*
     * On component unmount submit any unsubmitted changes so that changed ("dirty") fields
     * get persisted. Said differently this effects prevents data loss of unsaved changes.
     */
    useEffect(() => () => {
        shouldSubmitRef.current && submitDebouncedRef.current.flush()
    }, [])
    return null
}

export default EditFormAutoSave