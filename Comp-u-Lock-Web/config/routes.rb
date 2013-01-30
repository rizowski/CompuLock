CompULockWeb::Application.routes.draw do
  get "computer_accounts/index"

  get "computer_accounts/edit"

  get "computers/index"

  get "computers/edit"

  get "account/index"

  get "account/edit"

  get "home/index"

  root :to => "home#index"
  devise_for :users
  devise_scope :user do 
    match '/users/sign_out' => 'devise/sessions#destroy', :as => :destroy_user_session
  end

  match ':controller(/:action(/:id))(.:format)'
end
