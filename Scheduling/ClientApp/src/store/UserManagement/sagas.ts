import Cookies from "js-cookie";
import { put, takeEvery, all } from "redux-saga/effects";
import { getUsersData } from "../../webAPI/users";
import { createUser } from "../../webAPI/createUser";
import { removeUser } from "../../webAPI/removeUser";
import { getAllTeams } from "../../webAPI/teams";
import { getPermissions } from "../../webAPI/permissions";
import { Permission, Team, UserData } from "../User/types";
import { actionCreators } from "./actions";
import * as actions from "./actions"
import { editUser } from "../../webAPI/editUser";
import { EditUserResponse } from "./types";

export default function* watchUserManagementSagas() {
    yield all([
        takeEvery('REQUESTED_USERS', receiveUsersSaga),
        takeEvery('REQUESTED_CREATE_USER', createUserSaga),
        takeEvery('REQUESTED_DELETE_USER', removeUserSaga),
        takeEvery('REQUESTED_TEAMS', receiveTeamsSaga),
        takeEvery('REQUESTED_PERMISSIONS', receivePermissionsSaga),
        takeEvery('REQUESTED_EDIT_USER', editUserSaga)
    ]);
}

function* receiveTeamsSaga(action: actions.ReceivedTeamsAction) {
    const token = Cookies.get('token');
    if (token) {
        try {
            const response: Team[] = yield getAllTeams(token).then(response => response.data.getTeams);
            console.log(response);
            yield put(actionCreators.receivedTeams(response));
        } catch {
            yield put(actionCreators.accessDenied());
        }
    }
    else
        yield put(actionCreators.accessDenied());
}

function* receiveUsersSaga(action: actions.ReceivedUsersDataAction) {
    const token = Cookies.get('token');
    if (token) {
        try {
            const response: UserData[] = yield getUsersData(token).then(response => response.data.getUsers);
            console.log(response);
            yield put(actionCreators.receivedUsersData(response));
        } catch {
            yield put(actionCreators.accessDenied());
        }
    }
    else
        yield put(actionCreators.accessDenied());
}

function* createUserSaga(action: actions.UserCreatedAction) {
    const token = Cookies.get('token');
    if (token) {
        try {
            const response: UserData = yield createUser(
                action.payload!.name, action.payload!.surname,
                action.payload!.email, action.payload!.position, action.payload!.department,
                action.payload!.userPermissions.map((up) => up.permission.id),
                action.payload!.team.id, token)
                .then(response => response.data);
            console.log(response);
            yield put(actionCreators.createUser(response));
        } catch {
            yield put(actionCreators.accessDenied());
        }
    }
    else
        yield put(actionCreators.accessDenied());
}

function* removeUserSaga(action: actions.UserDeletedAction) {
    const token = Cookies.get('token');
    if (token) {
        try {
            const response: UserData = yield removeUser(action.payload, token);
            console.log(response);
            yield put(actionCreators.deleteUser(action.payload));
        } catch {
            yield put(actionCreators.accessDenied());
        }
    }
    else
        yield put(actionCreators.accessDenied());
}

function* receivePermissionsSaga(action: actions.ReceivedPermissionsAction) {
    const token = Cookies.get('token');
    if (token) {
        try {
            const response: Permission[] = yield getPermissions(token).then(response => response.data.getAllPermissions);
            console.log(response);
            yield put(actionCreators.receivedPermissions(response));
        } catch {
            yield put(actionCreators.accessDenied());
        }
    }
    else
        yield put(actionCreators.accessDenied());
}

function* editUserSaga(action: actions.RequestedEditUserAction) {
    const token = Cookies.get('token');
    if (token) {
        try {
            const response: EditUserResponse = yield editUser(action.payload.user, token);
            console.log(response);
            yield put(actionCreators.editedUserSuccess(response.message));
        } catch {
            yield put(actionCreators.accessDenied());
        }
    }
    else
        yield put(actionCreators.accessDenied());
}

