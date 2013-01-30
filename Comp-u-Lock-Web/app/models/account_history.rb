class AccountHistory < ActiveRecord::Base
  attr_accessible :account_id, :domain, :title, :last_visited, :url, :visit_count

  validates :domain, :presence => true

  belongs_to :account
end
