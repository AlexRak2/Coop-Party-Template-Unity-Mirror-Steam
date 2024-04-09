# Coop Party Template for Unity with Mirror and Steam

This is an advanced cooperative party system template designed for Unity, utilizing Mirror for networking and Steamworks for Steam integration.

I've developed this template for a project I'm currently working on and decided to share it with the community for free. This template provides a cooperative main menu system. You can create a party, join an existing party, find a match, or start in single-player mode.

One of the features of this template is its integration with Steam. You'll find a friends list menu where you can invite your Steam friends to join your party. All lobby connections are handled using Steam lobbies.

## Testing Locally

If you want to test this template locally using Parallel Sync, follow these steps:

- Change the Transport on `MyNetworkManager` to KCP Transport.
- Disable the FizzySteam component.

Vice versa, if you wish to utilize Steam integration, revert these changes. This is mainly to test locally with two instances or use Steam to test with friends.

## License

This project is licensed under the MIT License. Feel free to use it for your projects, modify it, or distribute it as per the terms of the license.

Thank you for checking out the Coop Party Template! If you have any questions or feedback, don't hesitate to reach out.
