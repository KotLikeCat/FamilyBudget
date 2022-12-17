import * as React from 'react';
import {ReactElement} from 'react';
import PropTypes from 'prop-types';
import {Link} from 'react-router-dom';
import {
    RaRecord,
    useRecordContext,
} from 'ra-core';

import {Button, ButtonProps} from "ra-ui-materialui"
import SourceIcon from '@mui/icons-material/Source';
import {useBasename} from "ra-core/src/routing/useBasename";

/**
 * Opens the Edit view for the current record.
 *
 * Reads the record and resource from the context.
 *
 * @example // basic usage
 * import { EditButton } from 'react-admin';
 *
 * const CommentEditButton = () => (
 *     <EditButton label="Edit comment" />
 * );
 */
export const DetailsButton = <RecordType extends RaRecord = any>(
    props: DetailsButtonButtonProps<RecordType>
) => {
    const {
        icon = defaultIcon,
        label = 'botsAttributes.logs',
        scrollToTop,
        ...rest
    } = props;
    const basename = useBasename();
    const record = useRecordContext(props);
    if (!record) return null;
    // @ts-ignore
    return (
        <Button
            component={Link}
            to={`${basename}/budgetDetails/${record.id}`}
            label={label}
            onClick={stopPropagation}
            {...(rest as any)}
        >
            {icon}
        </Button>
    );
};

// avoids using useMemo to get a constant value for the link state
const defaultIcon = <SourceIcon/>;

// useful to prevent click bubbling in a datagrid with rowClick
const stopPropagation = (e: any) => e.stopPropagation();

interface Props<RecordType extends RaRecord = any> {
    icon?: ReactElement;
    label?: string;
    record?: RecordType;
    resource?: string;
    scrollToTop?: boolean;
}

export type DetailsButtonButtonProps<RecordType extends RaRecord = any> = Props<RecordType> &
    ButtonProps;

DetailsButton.propTypes = {
    icon: PropTypes.element,
    label: PropTypes.string,
    record: PropTypes.any,
    scrollToTop: PropTypes.bool,
};